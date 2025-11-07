namespace Net.Belt.Comparisons;

/// <summary>
/// Provides utility methods for creating chainable comparison logic to sort objects of a specified type.
/// </summary>
/// <typeparam name="T">The type of objects to be compared.</typeparam>
public class Cmp<T>
{
	/// <summary>
	/// Creates a <see cref="ChainableComparer{T}"/> using the specified comparison logic and sort direction.
	/// </summary>
	/// <param name="next">The comparison logic to use for comparing objects of type <typeparamref name="T"/>.</param>
	/// <param name="sortDirection">The sort direction, either <see cref="Direction.Ascending"/> or <see cref="Direction.Descending"/>. Defaults to <see cref="Direction.Ascending"/>.</param>
	/// <returns>A <see cref="ChainableComparer{T}"/> that uses the specified comparison logic and sort direction.</returns>
	public static ChainableComparer<T> By(Comparison<T> next, Direction sortDirection = Direction.Ascending) =>
		new ComparisonComparer<T>(next, sortDirection);

	/// <summary>
	/// Creates a <see cref="ChainableComparer{T}"/> using the specified key selector and sort direction.
	/// </summary>
	/// <param name="keySelector">A function to extract a key from an element of type <typeparamref name="T"/>.</param>
	/// <param name="sortDirection">The sort direction, either <see cref="Direction.Ascending"/> or <see cref="Direction.Descending"/>. Defaults to <see cref="Direction.Ascending"/>.</param>
	/// <typeparam name="TKey">The type of the key returned by the key selector.</typeparam>
	/// <returns>A <see cref="ChainableComparer{T}"/> that uses the specified key selector and sort direction.</returns>
	public static ChainableComparer<T> By<TKey>(Func<T, TKey> keySelector,
		Direction sortDirection = Direction.Ascending) => new SelectorComparer<T, TKey>(keySelector, sortDirection);

	/// <summary>
	/// Creates a <see cref="ChainableComparer{T}"/> that utilizes the natural ordering operators of the type <typeparamref name="T"/>.
	/// </summary>
	/// <param name="sortDirection">The sort direction, either <see cref="Direction.Ascending"/> or <see cref="Direction.Descending"/>. Defaults to <see cref="Direction.Ascending"/>.</param>
	/// <returns>A <see cref="ChainableComparer{T}"/> that applies comparison logic based on the type's natural ordering operators.</returns>
	public static ChainableComparer<T> FromOperators(Direction sortDirection = Direction.Ascending) =>
		new OperatorComparer<T>(sortDirection);
}