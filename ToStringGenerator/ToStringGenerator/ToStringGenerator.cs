﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeAnalysisUtilities;
using LinqUtilities;

namespace ToStringGenerator;



[Generator]
public class ToStringGenerator : IIncrementalGenerator {

	public void Initialize(IncrementalGeneratorInitializationContext context) {

		IncrementalValuesProvider<INamedTypeSymbol> classDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (syntaxNode, _) => SyntaxFilter(syntaxNode),
				transform: static (syntaxContext, _) => SyntaxToSymbol(syntaxContext))
			.Where(static classDeclarationSyntax => classDeclarationSyntax is not null)!;

		IncrementalValueProvider<(Compilation, ImmutableArray<INamedTypeSymbol>)> unionClasses =
			context.CompilationProvider.Combine(classDeclarations.Collect());

		context.RegisterSourceOutput(unionClasses, Execute);
	}

	private static bool SyntaxFilter(SyntaxNode node) {
		return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
	}

	private static INamedTypeSymbol? SyntaxToSymbol(GeneratorSyntaxContext context) {

		if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax) {
			throw new InvalidOperationException($"{nameof(SyntaxFilter)} should filter out non {nameof(ClassDeclarationSyntax)} nodes.");
		}

		INamedTypeSymbol? typeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

		bool? hasGeneratorAttribute = typeSymbol?
			.GetAttributes()
			.Any(x => string.Equals(
				x.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), 
				GenerateToStringAttribute.Name));

		return hasGeneratorAttribute is true ? typeSymbol : null;
	}

	private static void Execute(SourceProductionContext context,
		(Compilation compilation, ImmutableArray<INamedTypeSymbol> classes) source) {

		IEnumerable<INamedTypeSymbol> distinctClasses = source.classes.Distinct(SymbolEqualityComparer.Default)
			.Where(x => x is not null)
			.Select(x => (INamedTypeSymbol)x!);

		foreach (INamedTypeSymbol typeSymbol in distinctClasses) {

			context.AddSource(NameGeneratedFile(typeSymbol), GenerateClassSource(typeSymbol));
		}
	}

	private static string NameGeneratedFile(INamedTypeSymbol typeSymbol) {

		int typeParameterCount = typeSymbol.TypeParameters.Length;

		return typeParameterCount == 0
			? $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}_ToString.g.cs"
			: $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}`{typeParameterCount}_ToString.g.cs";
	}

	private static string GenerateClassSource(INamedTypeSymbol classSymbol) {

		string className = classSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
		AttributeData attributeSymbol = classSymbol.GetAttribute<GenerateToStringAttribute>() ?? throw new("unreachable");

		string @namespace = classSymbol.ContainingNamespace is not null
			? $"namespace {classSymbol.ContainingNamespace.ToDisplayString()};\r\n"
			: string.Empty; 

		List<INamedTypeSymbol> nestingHierarchy = classSymbol.GetClassNesting();
		int classNestingDepth = nestingHierarchy.Count;
		string innerIndentation = new('\t', classNestingDepth);

		string nestedClassOpening = ClassNesting.GenerateNestedClassOpening(classSymbol);
		string nestedClassClosing = ClassNesting.GenerateNestedClassClosing(classSymbol);

		IEnumerable<ISymbol> membersToPrint = classSymbol
			.GetMembers()
			.Where(symbol => MemberMeetsInclusionRules(symbol, attributeSymbol))
			.ToImmutableArray();

		string toStringLines = membersToPrint
			.Select(symbol => $"$\"{symbol.Name} = {CreateFormatter(symbol)}")
			.Join($", \" +\r\n{innerIndentation}\t\t");

		const char tab = '\t';

		StringBuilder stringBuilder = new(
			$$"""
			  // <auto-generated /> 

			  #pragma warning disable 1591

			  {{@namespace}}


			  {{nestedClassOpening}}
			  {{innerIndentation}}
			  {{innerIndentation}}public override string ToString() {
			  """);

		if (membersToPrint.IsEmpty()) {
			stringBuilder.Append($"\r\n{innerIndentation}\treturn \"{className} {{ }}\";\r\n");

		} else {
			stringBuilder.Append(
				$$"""
				  
				  {{innerIndentation}}
				  {{innerIndentation}}{{tab}}return "{{className}} {" +
				  {{innerIndentation}}{{tab}}{{tab}}{{toStringLines}} " +
				  {{innerIndentation}}{{tab}}{{tab}}" }";
				  
				  """);
		}

		stringBuilder.Append(
			$$"""
			  {{innerIndentation}}}
			  {{innerIndentation}}
			  {{nestedClassClosing}}
			  """);

		return stringBuilder.ToString();
	}

	private static bool MemberMeetsInclusionRules(ISymbol symbol, AttributeData generateToStringAttribute) {

		if (symbol.Kind is not (SymbolKind.Property or SymbolKind.Field or SymbolKind.Method)) {
			return false;
		}

		if (symbol is IMethodSymbol { Parameters.Length: > 0 }) {
			return symbol.GetAttribute<IncludeParameteredMethodAttribute>() is not null;
		}

		if (symbol.GetAttribute<ExcludeFromToStringAttribute>() is not null) {
			return false;
		}

		if (symbol.GetAttribute<IncludeInToStringAttribute>() is not null) {
			return true;
		}

		AccessModifier propertyPolicy = generateToStringAttribute.NamedArguments
			.FirstOrDefault(x => x.Key == "PropertyPolicy").Value.Value as AccessModifier? ?? new GenerateToStringAttribute().PropertyPolicy;

		AccessModifier fieldPolicy = generateToStringAttribute.NamedArguments
			.FirstOrDefault(x => x.Key == "FieldPolicy").Value.Value as AccessModifier? ?? new GenerateToStringAttribute().FieldPolicy;

		AccessModifier parameterlessMethodPolicy = generateToStringAttribute.NamedArguments
			.FirstOrDefault(x => x.Key == "ParameterlessMethodPolicy").Value.Value as AccessModifier? ?? new GenerateToStringAttribute().ParameterlessMethodPolicy;

		return symbol.Kind switch {
			SymbolKind.Property => propertyPolicy.Includes(symbol.GetAccessModifier()),
			SymbolKind.Field => fieldPolicy.Includes(symbol.GetAccessModifier()),
			SymbolKind.Method => parameterlessMethodPolicy.Includes(symbol.GetAccessModifier()),
			_ => throw new("Unreachable")
		};

		// todo implement .ToInstance or something similar because I think this way is cleaner
		GenerateToStringAttribute attributeInstance = generateToStringAttribute.ToInstance<GenerateToStringAttribute>();

		return symbol.Kind switch {
			SymbolKind.Property => attributeInstance.PropertyPolicy.Includes(symbol.GetAccessModifier()),
			SymbolKind.Field => attributeInstance.FieldPolicy.Includes(symbol.GetAccessModifier()),
			SymbolKind.Method => attributeInstance.ParameterlessMethodPolicy.Includes(symbol.GetAccessModifier()),
			_ => throw new("Unreachable")
		};
	}

	private static string CreateFormatter(ISymbol symbol) {

		// todo fix this, it is always null
		AttributeData? formatterAttribute = symbol.GetAttribute(typeof(ToStringFormatAttribute<>));

		if (formatterAttribute is null) {
			return $"{{{symbol.Name}}}";
		}

		return (formatterAttribute.ConstructorArguments.First().Value as string)!;
	}

}

// todo figure out if this works for file scoped types
// todo check when INamedTypeSymbol.ContainingNamespace is null
// todo check when the type isn't in a namespace