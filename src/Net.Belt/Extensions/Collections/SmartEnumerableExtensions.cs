using Net.Belt.Collections;

namespace Net.Belt.Extensions.Collections;

/// <summary>
/// Provides extension methods for working with <see cref="SmartEnumerable{T}"/> and <see cref="SmartEntry{T}"/>.
/// </summary>
public static class SmartEnumerableExtensions
{
	/// <summary>
	/// Wraps an <see cref="IEnumerable{T}"/> in a <see cref="SmartEnumerable{T}"/> to provide additional
	/// information about each element during enumeration, such as its index, whether it's the first element,
	/// and whether it's the last element.
	/// </summary>
	/// <param name="source">The source <see cref="IEnumerable{T}"/> to enumerate.</param>
	/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
	/// <returns>A <see cref="SmartEnumerable{T}"/> that provides enhanced enumeration over the source.</returns>
	public static IEnumerable<SmartEntry<T>> AsSmartEnumerable<T>(this IEnumerable<T> source) => new SmartEnumerable<T>(source);
}