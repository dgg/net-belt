using System.Reflection;

namespace Net.Belt.Reflection;

/// <summary>
/// Provides a high-performance, thread-safe, zero-allocation cache of custom
/// attributes of type <typeparamref name="TAttr"/> that specify <see cref="AttributeUsageAttribute.AllowMultiple"/> = <see langword="false"/>.
/// The cache is populated once per closed generic type using reflection and reused for all
/// subsequent accesses.
/// </summary>
/// <typeparam name="TAttr">The type of attribute to search for.</typeparam>
/// <remarks>
/// Because static fields in generic types are initialized once per closed generic
/// combination, each attribute lookup is performed only a single time. This makes
/// <see cref="Cache{TAttr}"/> an efficient mechanism for accessing attribute metadata
/// without repeated reflection calls.
/// <para>Can be used to get the same caching benefits as <see cref="CachedAttribute{TAttr}"/> for scenarios in which
/// inheritance os not possible.</para>
/// </remarks>
public static class Cache<TAttr> where TAttr : Attribute
{
	/// <inheritdoc cref="CachedAttribute{TAttr}.Cache{TType}"/>
	public static class For<TType>
	{
		/// <inheritdoc cref="CachedAttribute{TAttr}.Cache{TType}.Value"/>
		public static TAttr? Value { get; } = typeof(TType).GetCustomAttribute<TAttr>(inherit: true);
	}

	/// <inheritdoc cref="CachedAttribute{TAttr}.ExactCache{TType}"/>
	public static class ForExact<TType>
	{
		/// <inheritdoc cref="CachedAttribute{TAttr}.ExactCache{TType}.Value"/>
		public static TAttr? Value { get; } = typeof(TType).GetCustomAttribute<TAttr>(inherit: false);
	}
}