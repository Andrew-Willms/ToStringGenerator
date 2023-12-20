using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// <param name="innerClass"></param>
	/// <param name="initialIndentLevel"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static string GenerateNestedClassOpening(INamedTypeSymbol innerClass, int initialIndentLevel = 0) {

		IEnumerable<INamedTypeSymbol> nestingHierarchy = innerClass.GetClassNesting();

		StringBuilder stringBuilder = new();

		int currentIndentationLevel = initialIndentLevel;

		foreach (INamedTypeSymbol classSymbol in nestingHierarchy) {

			string className = classSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

			stringBuilder.Append('\t', currentIndentationLevel);
			stringBuilder.Append("partial class ");
			stringBuilder.Append(className);
			stringBuilder.Append(" {");

			currentIndentationLevel++;

			if (currentIndentationLevel < initialIndentLevel + nestingHierarchy.Count()) {
				stringBuilder.Append("\r\n");
				stringBuilder.Append('\t', currentIndentationLevel);
				stringBuilder.Append("\r\n");
			}
		}

		return stringBuilder.ToString();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="innerClass"></param>
	/// <param name="initialIndentLevel"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static string GenerateNestedClassClosing(INamedTypeSymbol innerClass, int initialIndentLevel = 0) {

		IEnumerable<INamedTypeSymbol> nestingHierarchy = innerClass.GetClassNesting();

		StringBuilder stringBuilder = new();

		int currentIndentationLevel = initialIndentLevel + nestingHierarchy.Count();

		while (currentIndentationLevel > initialIndentLevel) {

			currentIndentationLevel--;

			stringBuilder.Append('\t', currentIndentationLevel);
			stringBuilder.Append(" }");

			if (currentIndentationLevel > initialIndentLevel) {
				stringBuilder.Append("\r\n");
				stringBuilder.Append('\t', currentIndentationLevel);
			}
		}

		return stringBuilder.ToString();
	}

}