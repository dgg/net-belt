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
}