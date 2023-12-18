using System;
using Microsoft.CodeAnalysis;

namespace ToStringGenerator;



[Flags]
public enum AccessModifier {
	None              = 0b00000,
	Public            = 0b00001,
	Internal          = 0b00010,
	Protected         = 0b00100,
	ProtectedInternal = 0b01000,
	Private           = 0b10000,
	All               = 0b11111
}



public static class AccessModifierExtensions {

	public static AccessModifier GetAccessModifier(this ISymbol symbol) {

		return symbol.DeclaredAccessibility switch {
			Accessibility.Private => AccessModifier.Private,
			Accessibility.ProtectedAndInternal => AccessModifier.ProtectedInternal,
			Accessibility.Protected => AccessModifier.Protected,
			Accessibility.Internal => AccessModifier.Internal,
			Accessibility.Public => AccessModifier.Public,
			Accessibility.ProtectedOrInternal => throw new NotImplementedException(), // I guess this is only for partial methods where it's declared elsewhere?
			Accessibility.NotApplicable => throw new NotImplementedException(), // either infer the default or search related symbols
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	public static bool Includes(this AccessModifier accessModifier, AccessModifier otherAccessModifier) {

		return (accessModifier & otherAccessModifier) > 0;
	}

}