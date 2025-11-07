namespace Net.Belt.Comparisons;

/// <summary>
/// A generic equality comparer that uses a key selector function to determine
/// object equality and hash code generation. This class extends the functionality of
/// chainable equality comparers, allowing comparison logic based on a selected key.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey">The type of the key selected by the selector function.</typeparam>
public class SelectorEqualizer<T, TKey>(Func<T, TKey> selector) : ChainableEqualizer<T>
{
	private TKey keyFor(T obj) => selector(obj);

	/// <summary>
	/// Determines whether two objects of type <typeparamref name="T"/> are equal
	/// based on a comparison of their selected keys of type <typeparamref name="TKey"/>.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns><c>true</c> if the selected keys of both objects are equal; otherwise, <c>false</c>.</returns>
	protected override bool DoEquals(T x, T y)
	{
		TKey xKey = keyFor(x), yKey = keyFor(y);
		return EqualityComparer<TKey>.Default.Equals(xKey, yKey);
	}

	/// <summary>
	/// Generates a hash code for the specified object of type <typeparamref name="T"/>
	/// based on the hash code of its selected key of type <typeparamref name="TKey"/>.
	/// </summary>
	/// <param name="obj">The object for which the hash code is to be generated.</param>
	/// <returns>The hash code of the selected key of the given object.</returns>
	protected override int DoGetHashCode(T obj)
	{
		TKey key = keyFor(obj);
		return EqualityComparer<TKey>.Default.GetHashCode(key!);
	}
}

public static partial class ChainableExtensions
{
	/// <summary>
	/// Chains the current chainable equalizer with an additional key-based equalizer.
	/// This allows the equality comparison to include a new key selector logic for comparison.
	/// </summary>
	/// <param name="chainable">The current chainable equalizer instance to be extended.</param>
	/// <param name="keySelector">A function to select the key of type <typeparamref name="TKey"/> from objects of type <typeparamref name="T"/>.</param>
	/// <typeparam name="T">The type of objects to compare.</typeparam>
	/// <typeparam name="TKey">The type of the key used for comparison.</typeparam>
	/// <returns>The updated chainable equalizer instance, allowing further chaining operations.</returns>
	public static ChainableEqualizer<T>
		Then<T, TKey>(this ChainableEqualizer<T> chainable, Func<T, TKey> keySelector) =>
		chainable.Then(new SelectorEqualizer<T, TKey>(keySelector));
}