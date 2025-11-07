namespace Net.Belt.Comparisons;

/// <summary>
/// A comparer that allows sorting based on a specific key extracted from the objects being compared.
/// </summary>
/// <typeparam name="T">The type of objects to be compared.</typeparam>
/// <typeparam name="TKey">The type of the key used for comparison.</typeparam>
/// <param name="keySelector">A function that extracts the key from an object of type <typeparamref name="T"/>.</param>
/// <param name="sortDirection">Specifies the direction of the sort (ascending or descending). Defaults to <see cref="Direction.Ascending"/>.</param>
public class SelectorComparer<T, TKey>(Func<T, TKey> keySelector, Direction sortDirection = Direction.Ascending)
	: ChainableComparer<T>(sortDirection)
{
	/// <summary>
	/// Performs the core comparison logic between two objects of type <typeparamref name="T"/> based on the key extracted by the selector function.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>An integer that indicates the relative order of the objects being compared.
	/// A value less than zero indicates that <paramref name="x"/> is less than <paramref name="y"/>.
	/// A value of zero indicates that <paramref name="x"/> is equal to <paramref name="y"/>.
	/// A value greater than zero indicates that <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
	protected override int DoCompare(T x, T y)
	{
		Comparer<TKey> comparer = Comparer<TKey>.Default;
		return comparer.Compare(keySelector(x), keySelector(y));
	}

	/// <summary>
	/// Gets a delegate representing the comparison logic provided by this comparer.
	/// </summary>
	/// <remarks>
	/// The returned delegate can be used to perform comparisons directly using the logic defined
	/// within this instance of the <see cref="SelectorComparer{T, TKey}"/>.
	/// </remarks>
	public Comparison<T> Comparison => Compare;
}

public static partial class ChainableExtensions
{
	/// <summary>
	/// Adds an additional comparison logic based on a key selector function to the current chainable comparer.
	/// This method enables chaining of multiple comparers in sequence.
	/// </summary>
	/// <param name="chainable">The current chainable comparer to which the additional comparison logic will be added.</param>
	/// <param name="keySelector">A function that extracts a key from an object of type <typeparamref name="T"/> for comparison.</param>
	/// <typeparam name="T">The type of objects being compared.</typeparam>
	/// <typeparam name="TKey">The type of the key extracted for comparison.</typeparam>
	/// <returns>A new <see cref="ChainableComparer{T}"/> that incorporates the additional comparison logic.</returns>
	public static ChainableComparer<T> Then<T, TKey>(this ChainableComparer<T> chainable, Func<T, TKey> keySelector) => chainable.Then(new SelectorComparer<T, TKey>(keySelector));

	/// <summary>
	/// Adds a secondary level of comparison to the current chainable comparer by creating a new selector-based comparer.
	/// The objects are compared using the key extracted by the provided key selector function and the specified sort direction.
	/// </summary>
	/// <param name="chainable">The current instance of the chainable comparer to which this secondary comparison will be applied.</param>
	/// <param name="keySelector">A function that extracts the key on which to perform the comparison for the objects of type <typeparamref name="T"/>.</param>
	/// <param name="sortDirection">The direction in which to sort (ascending or descending). Defaults to ascending if not specified.</param>
	/// <typeparam name="T">The type of objects being compared.</typeparam>
	/// <typeparam name="TKey">The type of the key returned by the key selector function.</typeparam>
	/// <returns>A chainable comparer that applies the secondary comparison based on the specified key selector and sort direction.</returns>
	public static ChainableComparer<T> Then<T, TKey>(this ChainableComparer<T> chainable, Func<T, TKey> keySelector,
		Direction sortDirection) => chainable.Then(new SelectorComparer<T, TKey>(keySelector, sortDirection));
}