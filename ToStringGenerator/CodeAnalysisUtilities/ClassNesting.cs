using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodeAnalysisUtilities; 



public static class ClassNesting {

	// todo use cref to link to the appropriate data types
	/// <summary>
	/// Returns a List of INamedTypeSymbols representing the hierarchy of nesting.
	/// The first element in the list is the outermost class, the last element is the class this extension method is called on.
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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="nestingHierarchy"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static string GenerateNestedClassOpening(IEnumerable<INamedTypeSymbol> nestingHierarchy) {
		throw new NotImplementedException();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="nestingHierarchy"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static string GenerateNestedClassClosing(IEnumerable<INamedTypeSymbol> nestingHierarchy) {
		throw new NotImplementedException();
	}

}