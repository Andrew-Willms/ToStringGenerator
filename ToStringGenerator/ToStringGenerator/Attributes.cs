using System;

namespace ToStringGenerator;



/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public class GeneratorToStringAttribute : Attribute {

	// todo this doesn't feel quite right but I will leave it for now
	internal static readonly string Name = $"global::{typeof(GeneratorToStringAttribute).FullName!}";

}



/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ExcludeFromToStringAttribute : Attribute;



/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ToStringFormatAttribute<T> : Attribute {

	/// <summary>
	/// 
	/// </summary>
	/// <param name="formatter"></param>
	public ToStringFormatAttribute(Func<T, string> formatter) {

	}

}