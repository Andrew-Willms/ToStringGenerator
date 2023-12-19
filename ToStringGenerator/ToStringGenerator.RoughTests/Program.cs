using System;

namespace ToStringGenerator.RoughTests;



internal class Program {
	
	public static void Main() {

		TestClass myClass = new();

		Console.WriteLine(myClass.ToString());
		Console.WriteLine(myClass);
	}

}



[GenerateToString]
public class TestClass {

	public string TestString { get; set; } = "Hello World";

}