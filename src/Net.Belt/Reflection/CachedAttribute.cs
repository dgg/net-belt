using System.Reflection;

namespace Net.Belt.Reflection;

/// <summary>
/// Base class for custom attributes that specify <see cref="AttributeUsageAttribute.AllowMultiple"/> = <see langword="false"/>
/// and provides a thread-safe, allocation-free, per-type cache of a single <typeparamref name="TAttr"/> instance.
/// </summary>
/// <typeparam name="TAttr">The concrete attribute type being cached.</typeparam>
/// <remarks>
/// Because static fields in generic types are initialized once per closed generic combination,
/// each attribute lookup is performed only a single time. This makes <see cref="Cache{TType}"/> and
/// <see cref="ExactCache{TType}"/> an efficient mechanism for accessing attribute metadata without
/// repeated reflection calls.
/// <para>For scenarios in which inheritance is not possible, consider <see cref="Reflection.Cache{TAttr}"/>.</para>
/// </remarks>
public abstract class CachedAttribute<TAttr> : Attribute where TAttr : CachedAttribute<TAttr>
{
	/// <summary>
	/// Stores a custom attribute of type <typeparamref name="TAttr"/> applied to <typeparamref name="TType"/>,
	/// inspecting ancestors of <typeparamref name="TType"/>.
	/// </summary>
	/// <typeparam name="TType">The type to search for a custom attribute of type <typeparamref name="TAttr"/>.</typeparam>
	public static class Cache<TType>
	{
		/// <returns>A cached custom attribute that matches <typeparamref name="TAttr"/>, or <c>null</c> if no such attribute is found.</returns>
		public static TAttr? Value { get; } = typeof(TType).GetCustomAttribute<TAttr>(inherit: true);
	}
	
	/// <summary>
	/// Stores a custom attribute of type <typeparamref name="TAttr"/> applied to <typeparamref name="TType"/>,
	/// ignoring ancestors of <typeparamref name="TType"/>.
	/// </summary>
	/// <typeparam name="TType">The type to search for a custom attribute of type <typeparamref name="TAttr"/>.</typeparam>
	public static class ExactCache<TType>
	{
		/// <inheritdoc cref="Cache{TType}.Value"/>
		public static TAttr? Value { get; } = typeof(TType).GetCustomAttribute<TAttr>(inherit: false);
	}
}