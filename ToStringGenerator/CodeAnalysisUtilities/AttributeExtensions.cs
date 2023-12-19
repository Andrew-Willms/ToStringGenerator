using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CodeAnalysisUtilities; 



public static class AttributeExtensions {

	public static bool HasAttribute<TAttribute>(this ISymbol symbol) where TAttribute : Attribute {

		return symbol
			.GetAttributes()
			.Any(attributeData => string.Equals(
				attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				$"global::{typeof(TAttribute).Namespace}.{typeof(TAttribute).Name}"));
	}

	public static bool HasAttribute(this ISymbol symbol, Type attributeType) {

		if (typeof(Attribute).IsAssignableFrom(attributeType)) {
			throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		}

		return symbol
			.GetAttributes()
			.Any(attributeData => string.Equals(
				attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				$"global::{attributeType.Namespace}.{attributeType.Name}"));
	}



	public static AttributeData? GetAttribute<TAttribute>(this ISymbol symbol) where TAttribute : Attribute {

		foreach (AttributeData attributeData in symbol.GetAttributes()) {

			string? string1 = attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			string string2 = $"global::{typeof(TAttribute).Namespace}.{typeof(TAttribute).Name}";

			if (string.Equals(string1, string2)) {
				return attributeData;
			}
		}

		// todo check if this works with no namespace/a null namespace
		return symbol
			.GetAttributes()
			.FirstOrDefault(attributeData => string.Equals(
				attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), 
				$"global::{typeof(TAttribute).Namespace}.{typeof(TAttribute).Name}"));
	}

	// todo make analyzer to check and make sure attributeType inherits from Attribute
	public static AttributeData? GetAttribute(this ISymbol symbol, Type attributeType) {

		// todo see what sorts of debug messages we get when we pass in an improper type
		if (typeof(Attribute).IsAssignableFrom(attributeType)) {
			throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		}

		return symbol
			.GetAttributes()
			.FirstOrDefault(attributeData => string.Equals(
				attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				$"global::{attributeType.Namespace}.{attributeType.Name}"));
	}



	// todo figure out if this is desirable
	public static INamedTypeSymbol? GetAttributeSymbol<TAttribute>(this ISymbol symbol) where TAttribute : Attribute {

		return symbol.GetAttribute<TAttribute>()?.AttributeClass;
	}

	public static INamedTypeSymbol? GetAttributeSymbol(this ISymbol symbol, Type attributeType) {

		if (typeof(Attribute).IsAssignableFrom(attributeType)) {
			throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		}

		return symbol.GetAttribute(attributeType)?.AttributeClass;
	}



	public static ImmutableArray<AttributeData> GetAttributes<TAttribute>(this ISymbol symbol) where TAttribute : Attribute {

		return symbol
			.GetAttributes()
			.Where(attributeData => string.Equals(
				attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				$"global::{typeof(TAttribute).Namespace}.{typeof(TAttribute).Name}"))
			.ToImmutableArray();
	}

	public static ImmutableArray<AttributeData> GetAttributes(this ISymbol symbol, Type attributeType) {

		if (typeof(Attribute).IsAssignableFrom(attributeType)) {
			throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		}

		return symbol
			.GetAttributes()
			.Where(attributeData => string.Equals(
				attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				$"global::{attributeType.Namespace}.{attributeType.Name}"))
			.ToImmutableArray();
	}



	public static IEnumerable<INamedTypeSymbol> GetAttributeSymbols<TAttribute>(this ISymbol symbol) where TAttribute : Attribute {

		return symbol
			.GetAttributes<TAttribute>()
			.Select(attributeData => 
				attributeData.AttributeClass 
				?? throw new InvalidOperationException("The AttributeClass was null. This should not happen."));
	}

	public static IEnumerable<INamedTypeSymbol> GetAttributeSymbols(this ISymbol symbol, Type attributeType) {

		if (typeof(Attribute).IsAssignableFrom(attributeType)) {
			throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		}

		return symbol
			.GetAttributes(attributeType)
			.Select(attributeData =>
				attributeData.AttributeClass
				?? throw new InvalidOperationException("The AttributeClass was null. This should not happen."));
	}



	public static TAttribute ToInstance<TAttribute>(this AttributeData attributeData) where TAttribute : Attribute {
		throw new NotImplementedException();
	}

}