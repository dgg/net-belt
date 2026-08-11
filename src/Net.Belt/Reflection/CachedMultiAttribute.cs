using System.Collections.Immutable;
using System.Reflection;

namespace Net.Belt.Reflection;

/// <summary>
/// Base class for custom attributes that specify <see cref="AttributeUsageAttribute.AllowMultiple"/> = <see langword="true"/>
/// and provides a thread-safe, allocation-free, per-type cache of all <typeparamref name="TAttr"/> instances.
/// </summary>
/// <typeparam name="TAttr">The concrete attribute type being cached.</typeparam>
/// <remarks>
/// Because static fields in generic types are initialized once per closed generic combination,
/// each attribute lookup is performed only a single time. This makes <see cref="Cache{TType}"/> and
/// <see cref="ExactCache{TType}"/> an efficient mechanism for accessing attribute metadata without
/// repeated reflection calls.
/// <para>For scenarios in which inheritance is not possible, consider <see cref="MultiCache{TAttr}"/>.</para>
/// </remarks>
public abstract class CachedMultiAttribute<TAttr> : Attribute where TAttr : CachedMultiAttribute<TAttr>
{
	/// <summary>
	/// Stores a collection of custom attributes of type <typeparamref name="TAttr"/> applied to <typeparamref name="TType"/>
	/// and its ancestors, most-derived first.
	/// </summary>
	/// <typeparam name="TType">The type to search for custom attributes of type <typeparamref name="TAttr"/>.</typeparam>
	public static class Cache<TType>
	{
		/// <returns>A collection of the custom attributes that match <typeparamref name="TAttr"/>,
		/// or an empty collection if no such attributes exist.</returns>
		public static ImmutableArray<TAttr> Values { get; } = [..typeof(TType).GetCustomAttributes<TAttr>(inherit: true)];

		/// <returns>The first (most-derived first) of the custom attributes that match <typeparamref name="TAttr"/>,
		/// or an <c>null</c> if no such attributes exist.</returns>
		public static TAttr? First { get; } = Values.IsEmpty ? null : Values[0];
	}
	
	/// <summary>
	/// Stores a collection of custom attributes of type <typeparamref name="TAttr"/> applied to <typeparamref name="TType"/>
	/// directly, ignored ancestors.
	/// </summary>
	/// <typeparam name="TType">The type to search for custom attributes of type <typeparamref name="TAttr"/>.</typeparam>
	public static class ExactCache<TType>
	{
		/// <inheritdoc cref="Cache{TType}.Values"/>
		public static ImmutableArray<TAttr> Values { get; } = [..typeof(TType).GetCustomAttributes<TAttr>(inherit: false)];

		/// <inheritdoc cref="Cache{TType}.First"/>
		public static TAttr? First { get; } = Values.IsEmpty ? null : Values[0];
	}
}