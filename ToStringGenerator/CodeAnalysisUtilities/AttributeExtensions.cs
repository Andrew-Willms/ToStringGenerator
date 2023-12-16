using System.Linq;
using Microsoft.CodeAnalysis;

namespace CodeAnalysisUtilities; 



public static class AttributeExtensions {

	public static bool HasAttribute<TAttribute>(this ISymbol symbol) {

		return symbol
			.GetAttributes()
			.Any(attribute => symbol.Equals(attribute?.AttributeClass));
	}

}