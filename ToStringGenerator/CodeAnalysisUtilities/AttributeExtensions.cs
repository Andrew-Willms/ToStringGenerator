using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CodeAnalysisUtilities; 



public static class AttributeExtensions {

	public static bool HasAttribute<TAttribute>(this ISymbol symbol) {

		return symbol
			.GetAttributes()
			.Any(attribute => symbol.Equals(attribute?.AttributeClass));

		return symbol
			.GetAttributes()
			.Any(attributeData => string.Equals(
				attributeData.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				$"global::{typeof(TAttribute).Namespace}.{typeof(TAttribute).Name}"));
	}

	public static AttributeData GetAttribute<TAttribute>(this ISymbol symbol) where TAttribute : Attribute {

		// todo see what sorts of debug messages we get when we pass in an improper type
		//if (typeof(Attribute).IsAssignableFrom(attributeType)) {
		//	throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		//}

		// todo test this with generic attributes
		return symbol.GetAttributes(typeof(TAttribute)).First();

		//INamedTypeSymbol? attributeSymbol = classSymbol
		//	.GetAttributes()
		//	.Select(attributeData => attributeData.AttributeClass)
		//	.First(attribute => string.Equals(
		//		attribute?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), GenerateToStringAttribute.Name));
	}

	public static INamedTypeSymbol GetAttributeSymbol<TAttribute>(this ISymbol symbol) where TAttribute : Attribute {

		// todo see what sorts of debug messages we get when we pass in an improper type
		//if (typeof(Attribute).IsAssignableFrom(attributeType)) {
		//	throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		//}

		return symbol
			.GetAttributes(typeof(TAttribute))
			.Select(attributeData => attributeData.AttributeClass ?? throw new("AttributeData.AttributeClass should not be null."))
			.First();
	}

	public static ImmutableArray<AttributeData> GetAttributes(this ISymbol symbol, Type attributeType) {

		if (typeof(Attribute).IsAssignableFrom(attributeType)) {
			throw new ArgumentException("Must inherit from the Attribute class.", nameof(attributeType));
		}

		return symbol
			.GetAttributes()
			.Select(attributeData => attributeData.AttributeClass)
	}

	// todo figure out if this is desirable
	public static IEnumerable<INamedTypeSymbol> GetAttributeSymbols(this ISymbol symbol) {

		return symbol
			.GetAttributes()
			.Select(attributeData => 
				attributeData.AttributeClass 
				?? throw new InvalidOperationException("The AttributeClass was null. This should not happen."));
	}

	// todo GetGenericAttribute/HasGenericAttribute

}




public class Test {


	public static void TestMethod() {

		ISymbol testSymbol = null!;

		testSymbol.GetAttribute<TestAttribute<TestParentClass>>(typeof(object));


	}


}


public abstract class TestParentClass;

public class TestAttribute<T> : Attribute where T : TestParentClass {

}