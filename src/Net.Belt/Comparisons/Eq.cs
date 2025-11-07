namespace Net.Belt.Comparisons;

/// <summary>
/// Provides utility methods for working with equality comparison logic.
/// This class is designed to facilitate the definition and handling of equality and hashing mechanisms
/// for objects of a specified type.
/// </summary>
/// <typeparam name="T">The type of objects for which equality and hash code logic is defined.</typeparam>
public static class Eq<T>
{
	/// <summary>
	/// Creates a chainable equality comparer that uses the specified equality logic and hash code generator.
	/// </summary>
	/// <param name="equals">A delegate that defines the equality logic, which compares two instances of type <typeparamref name="T"/> and returns a boolean indicating whether they are equal.</param>
	/// <param name="hasher">A delegate that generates a hash code for an instance of type <typeparamref name="T"/>.</param>
	/// <returns>A chainable equality comparer that applies the specified equality and hash code logic.</returns>
	public static ChainableEqualizer<T> By(Func<T, T, bool> equals, Func<T, int> hasher) =>
		new DelegatedEqualizer<T>(equals, hasher);

	/// <summary>
	/// Creates a chainable equality comparer by selecting a key from objects of type <typeparamref name="T"/>
	/// using the provided key selector.
	/// </summary>
	/// <param name="keySelector">A function that extracts a key of type <typeparamref name="TKey"/> from an object of type <typeparamref name="T"/>.
	/// The equality and hashing logic will be derived from this key.</param>
	/// <typeparam name="TKey">The type of the key used for equality and hash code computation.</typeparam>
	/// <returns>A chainable equality comparer that applies equality and hash code logic based on the extracted key.</returns>
	public static ChainableEqualizer<T> By<TKey>(Func<T, TKey> keySelector)
	{
		return new SelectorEqualizer<T, TKey>(keySelector);
	}
}