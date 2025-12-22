using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Net.Belt.Extensions.Attributes;

/// <summary>
/// Provides extension methods for working with custom attributes.
/// </summary>
public static class AttributeExtensions
{
	/// <summary>
	/// Determines whether the custom attribute of the specified type or its derived types is applied to the type of the object.
	/// </summary>
	/// <param name="instance">The object from which type to inspect.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
	/// <returns><c>true</c> if the attribute is applied to the specified object; otherwise, <c>false</c>.</returns>
	public static bool HasAttribute<TAttribute>(this object instance, bool inherit = false)
		where TAttribute : Attribute =>
		instance.GetType().HasAttribute<TAttribute>(inherit);

	/// <summary>
	/// Determines whether the custom attribute of the specified type or its derived types is applied to the specified member.
	/// </summary>
	/// <param name="memberInfo">The member to inspect.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
	/// <returns><c>true</c> if the attribute is applied to the specified member; otherwise, <c>false</c>.</returns>
	public static bool HasAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = false)
		where TAttribute : Attribute =>
		memberInfo.IsDefined(typeof(TAttribute), inherit);

	/// <summary>
	/// Determines whether the custom attribute of the specified type or its derived types is applied to the specified parameter.
	/// </summary>
	/// <param name="parameterInfo">The parameter to inspect.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
	/// <returns><c>true</c> if the attribute is applied to the specified parameter; otherwise, <c>false</c>.</returns>
	public static bool HasAttribute<TAttribute>(this ParameterInfo parameterInfo, bool inherit = false)
		where TAttribute : Attribute =>
		parameterInfo.IsDefined(typeof(TAttribute), inherit);

	/// <summary>
	/// Retrieves the custom attribute of the specified type applied to the type of the object.
	/// </summary>
	/// <param name="instance">The object from which type to inspect.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to retrieve.</typeparam>
	/// <returns>The attribute of the specified type if found; otherwise, throws an <see cref="InvalidOperationException"/>.</returns>
	/// <exception cref="InvalidOperationException">The specified attribute is not found.</exception>
	public static TAttribute GetAttribute<TAttribute>(this object instance, bool inherit = false)
		where TAttribute : Attribute => instance.GetType().GetAttribute<TAttribute>(inherit);

	/// <summary>
	/// Retrieves the custom attribute of the specified type applied to the specified member.
	/// </summary>
	/// <param name="memberInfo">The member to inspect.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to retrieve.</typeparam>
	/// <returns>The attribute of the specified type if found; otherwise, throws an <see cref="InvalidOperationException"/>.</returns>
	/// <exception cref="InvalidOperationException">The specified attribute is not found.</exception>
	public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = false)
		where TAttribute : Attribute =>
		memberInfo.GetCustomAttribute<TAttribute>(inherit) ?? throw new InvalidOperationException(
			$"Attribute {typeof(TAttribute).Name} is not found on {memberInfo.DeclaringType?.Name}.{memberInfo.Name}.");

	/// <summary>
	/// Retrieves the custom attribute of the specified type applied to the specified parameter.
	/// </summary>
	/// <param name="parameterInfo">The parameter to inspect.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to retrieve.</typeparam>
	/// <returns>The attribute of the specified type if found; otherwise, throws an <see cref="InvalidOperationException"/>.</returns>
	/// <exception cref="InvalidOperationException">The specified attribute is not found.</exception>
	public static TAttribute GetAttribute<TAttribute>(this ParameterInfo parameterInfo, bool inherit = false)
		where TAttribute : Attribute =>
		parameterInfo.GetCustomAttribute<TAttribute>(inherit) ?? throw new InvalidOperationException(
			$"Attribute {typeof(TAttribute).Name} is not found on {parameterInfo.Member.DeclaringType?.Name}.{parameterInfo.Member.Name}.");
	
	/// <summary>
	/// Attempts to retrieve a custom attribute of the specified type applied to the type of the object.
	/// </summary>
	/// <param name="instance">The object from which type to inspect.</param>
	/// <param name="attribute">When this method returns, contains the custom attribute applied to the type of the <paramref name="instance"/>, if it exists; otherwise, <c>null</c>.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
	/// <returns><c>true</c> if the attribute is applied to the type of the instance; otherwise, <c>false</c>.</returns>
	public static bool TryGetAttribute<TAttribute>(this object instance, [NotNullWhen(true)] out TAttribute? attribute, bool inherit = false) where TAttribute : Attribute =>
		instance.GetType().TryGetAttribute(out attribute, inherit);

	/// <summary>
	/// Attempts to retrieve a custom attribute of the specified type applied to a member.
	/// </summary>
	/// <param name="memberInfo">The member to inspect.</param>
	/// <param name="attribute">When this method returns, contains the custom attribute applied to the member if it exists; otherwise, <c>null</c>.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
	/// <returns><c>true</c> if the attribute is applied to the member; otherwise, <c>false</c>.</returns>
	public static bool TryGetAttribute<TAttribute>(this MemberInfo memberInfo, [NotNullWhen(true)] out TAttribute? attribute,
		bool inherit = false)
		where TAttribute : Attribute => (attribute = memberInfo.GetCustomAttribute<TAttribute>(inherit)) != null;
	
	/// <summary>
	/// Attempts to retrieve a custom attribute of the specified type applied to a parameter.
	/// </summary>
	/// <param name="parameterInfo">The parameter to inspect.</param>
	/// <param name="attribute">When this method returns, contains the custom attribute applied to the parameter if it exists; otherwise, <c>null</c>.</param>
	/// <param name="inherit">Specifies whether to search for the attribute in the inheritance chain.</param>
	/// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
	/// <returns><c>true</c> if the attribute is applied to the parameter; otherwise, <c>false</c>.</returns>
	public static bool TryGetAttribute<TAttribute>(this ParameterInfo parameterInfo, [NotNullWhen(true)] out TAttribute? attribute, bool inherit = false)
		where TAttribute : Attribute => (attribute = parameterInfo.GetCustomAttribute<TAttribute>(inherit)) != null;

	/// <summary>
	/// Retrieves a collection of custom attributes of a specified type that are applied to the type of the object and, optionally, inspects the ancestors of that member.
	/// </summary>
	/// <param name="instance">The object which type is inspected.</param>
	/// <param name="inherit">Specifies whether to include attributes inherited from the base classes.</param>
	/// <typeparam name="TAttribute">The type of the attribute to retrieve.</typeparam>
	/// <returns>An array of attributes of type <typeparamref name="TAttribute"/> applied to the object's type.</returns>
	public static TAttribute[] GetAttributes<TAttribute>(this object instance, bool inherit = false)
		where TAttribute : Attribute =>
		instance.GetType().GetCustomAttributes<TAttribute>(inherit).ToArray();
}