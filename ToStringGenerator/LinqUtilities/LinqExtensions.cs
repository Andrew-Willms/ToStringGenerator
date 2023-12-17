using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqUtilities; 



// todo from elsewhere and not used here, find a good home for these
public static class LinqExtensions {

	public static IEnumerable<T> Exclude<T>(this IEnumerable<T> enumerable, T excludedValue) {
		return enumerable.Where(x => !Equals(x, excludedValue));
	}

	public static IEnumerable<T> Exclude<T>(this IEnumerable<T> enumerable, params T[] excludedValues) {
		return enumerable.Where(x => !excludedValues.Contains(x));
	}

	public static IEnumerable<T> Exclude<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) {
		return enumerable.Where(x => !predicate(x));
	}

}