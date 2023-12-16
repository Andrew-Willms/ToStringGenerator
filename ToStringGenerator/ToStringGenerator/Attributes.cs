using System;

namespace ToStringGenerator;



/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public class GenerateToStringAttribute : Attribute {

	// todo this doesn't feel quite right but I will leave it for now
	internal static readonly string Name = $"global::{typeof(GenerateToStringAttribute).FullName!}";

	/// <summary>
	/// 
	/// </summary>
	public bool MultiLine { get; set; } = false;

	[Flags]
	public enum AccessModifier {
		None                       = 0b00000,
		Public                     = 0b00001,
		Internal                   = 0b00010,
		Protected                  = 0b00100,
		ProtectedInternal          = 0b01000,
		Private                    = 0b10000,
		All                        = 0b11111
	}

	/// <summary>
	/// 
	/// </summary>
	public AccessModifier IncludeProperties { get; set; } = AccessModifier.Public;

	/// <summary>
	/// 
	/// </summary>
	public AccessModifier IncludeFields { get; set; } = AccessModifier.Public;

	/// <summary>
	/// 
	/// </summary>
	public AccessModifier IncludeStaticMembers { get; set; } = AccessModifier.None;

	/// <summary>
	/// 
	/// </summary>
	public AccessModifier IncludeParameterlessFunctions { get; set; } = AccessModifier.None;

}



/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class ExcludeFromToStringAttribute : Attribute;



/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class IncludeInToStringAttribute : Attribute;



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