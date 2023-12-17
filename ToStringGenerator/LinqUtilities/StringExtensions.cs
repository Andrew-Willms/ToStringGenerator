using System.Collections.Generic;

namespace LinqUtilities;



public static class StringExtensions {

	public static string Join(this IEnumerable<string> enumerable) {
		return string.Join(string.Empty, enumerable);
	}

	public static string Join(this IEnumerable<string> enumerable, string separator) {
		return string.Join(separator, enumerable);
	}

	public static string LowerFirstLetter(this string text) {

		return string.IsNullOrWhiteSpace(text)
			? text
			: char.ToLower(text[0]) + text.Substring(1);
	}

}