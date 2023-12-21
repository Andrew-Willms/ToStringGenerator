using System;

namespace ToStringGenerator.RoughTests;



internal class Program {
	
	public static void Main() {

		TestClass myClass = new();

		Console.WriteLine(myClass.ToString());
		Console.WriteLine(myClass);
	}

}



[GenerateToString(PropertyPolicy = AccessModifier.All)]
public partial class TestClass {

	public string TestString1 { get; set; } = "Hello World";

	public string TestString2 { get; set; } = "Hello Other World";

	[ToStringFormat<string>("{TestString3}!")]
	public string TestString3 { get; set; } = "Yo dawg";

}