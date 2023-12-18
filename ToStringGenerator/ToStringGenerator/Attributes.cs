using System;

namespace ToStringGenerator;



/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public class GenerateToStringAttribute : Attribute {

	// todo this doesn't feel quite right but I will leave it for now
	internal static readonly string Name = $"global::{typeof(GenerateToStringAttribute).FullName!}";

	//todo add support
	/// <summary>
	/// 
	/// </summary>
	public bool MultiLine { get; set; } = false;

	//todo add support
	/// <summary>
	/// 
	/// </summary>
	public bool IncludeNamespace { get; set; } = false;

	//todo add support
	/// <summary>
	/// 
	/// </summary>
	public bool IncludeAssembly { get; set; } = false;

	/// <summary>
	/// 
	/// </summary>
	public AccessModifier PropertyPolicy { get; set; } = AccessModifier.Public;

	/// <summary>
	/// 
	/// </summary>
	public AccessModifier FieldPolicy { get; set; } = AccessModifier.Public;

	// todo add support
	/// <summary>
	/// 
	/// </summary>
	public AccessModifier IncludeStaticMembersPolicy { get; set; } = AccessModifier.None;

	/// <summary>
	/// 
	/// </summary>
	public AccessModifier ParameterlessMethodPolicy { get; set; } = AccessModifier.None;

}



/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class ExcludeFromToStringAttribute : Attribute;



// todo analyzer to ensure that this is only applied to parameterless methods
/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class IncludeInToStringAttribute : Attribute;



// todo add support for parametered methods, presumably the values have to be provided here
public class IncludeParameteredMethodAttribute : Attribute;



// todo ensure the type of the return type of the member matches the type parameter
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class ToStringFormatAttribute<T> : Attribute {

	/// <summary>
	/// 
	/// </summary>
	/// <param name="formatter"></param>
	public ToStringFormatAttribute(Func<T, string> formatter) {

	}

}