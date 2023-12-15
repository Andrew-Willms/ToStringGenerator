using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace CodeAnalysisUtilities; 



public static class ClassNesting {

	/// <summary>
	/// 
	/// </summary>
	/// <param name="classSymbol"></param>
	/// <returns></returns>
	public static List<INamedTypeSymbol> GetClassNesting(this INamedTypeSymbol classSymbol) {

		if (classSymbol.ContainingType is null) {
			return new() { classSymbol };
		}

		List<INamedTypeSymbol> classNesting = GetClassNesting(classSymbol.ContainingType);

		classNesting.Add(classSymbol);

		return classNesting;
	}

}