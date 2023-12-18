using System;
using System.Diagnostics;

namespace RoughWork;



public class Program {

	public static void Main(params string[] args) {

		//throw new UnreachableException();

		Console.WriteLine(new TestToStringType());

		//string text = "2";
		//Console.WriteLine(text.Substring(1));
		//Console.WriteLine(text[1..]);
		//Console.WriteLine(text);

		//Console.WriteLine(typeof(GenericAttribute<int>).Namespace);
		//Console.WriteLine(typeof(GenericAttribute<int>).Name);
		//Console.WriteLine(typeof(GenericAttribute<>).Name);
		//Console.WriteLine(typeof(GenericAttribute<>).ContainsGenericParameters);
		//Console.WriteLine(typeof(GenericAttribute<int>).ContainsGenericParameters);
		//Console.WriteLine(typeof(GenericAttribute<>).IsConstructedGenericType);
		//Console.WriteLine(typeof(GenericAttribute<int>).IsConstructedGenericType);
		//Console.WriteLine(typeof(GenericAttribute<>).IsGenericType);
		//Console.WriteLine(typeof(GenericAttribute<int>).IsGenericType);
		//Console.WriteLine(typeof(GenericAttribute<>).IsGenericTypeDefinition);
		//Console.WriteLine(typeof(GenericAttribute<int>).IsGenericTypeDefinition);

	}

}



public record TestToStringType {

	public int Number { get; set; }

}

public class GenericAttribute<T> : Attribute {

}