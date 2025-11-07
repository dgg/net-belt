namespace Net.Belt.Comparisons;

/// <summary>
/// Provides a comparer implementation that uses a specified comparison delegate
/// to compare two objects of a specified type, with support for sorting direction (ascending or descending).
/// </summary>
/// <typeparam name="T">The type of objects to be compared.</typeparam>
public class ComparisonComparer<T>(Comparison<T> comparison, Direction sortDirection = Direction.Ascending)
	: ChainableComparer<T>(sortDirection)
{
	/// <summary>
	/// Gets the comparison delegate used to compare two objects of the specified type.
	/// This delegate defines the logic for comparing objects within the comparer.
	/// </summary>
	public Comparison<T> Comparison => Compare;

	/// <summary>
	/// Compares two objects of the specified type using the configured comparison delegate.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>
	/// A value that indicates the relative order of the objects being compared:
	/// a negative integer if <paramref name="x"/> is less than <paramref name="y"/>,
	/// zero if they are equal, or a positive integer if <paramref name="x"/> is greater than <paramref name="y"/>.
	/// </returns>
	protected override int DoCompare(T x, T y) => comparison(x, y);
}

/// <summary>
/// 
/// </summary>
public static partial class ChainableExtensions
{
	/// <summary>
	/// Adds a subsequent comparison to a chainable comparer using the specified comparison delegate.
	/// </summary>
	/// <param name="chainable">The current chainable comparer to extend.</param>
	/// <param name="next">The comparison delegate to use for the next comparison in the chain.</param>
	/// <typeparam name="T">The type of objects to be compared.</typeparam>
	/// <returns>
	/// A new chainable comparer that incorporates the specified comparison delegate
	/// into the comparison sequence.
	/// </returns>
	public static ChainableComparer<T> Then<T>(this ChainableComparer<T> chainable, Comparison<T> next) =>
		chainable.Then(new ComparisonComparer<T>(next));

	/// <summary>
	/// Adds a subsequent comparison to the existing chainable comparer, using the specified comparison delegate.
	/// </summary>
	/// <typeparam name="T">The type of objects to be compared.</typeparam>
	/// <param name="chainable">The existing chainable comparer to extend.</param>
	/// <param name="next">The next comparison delegate used for additional comparisons.</param>
	/// <param name="sortDirection">Sorting direction (ascending or descending)</param>
	/// <returns>
	/// A new chainable comparer that includes the additional comparison logic specified by <paramref name="next"/>.
	/// </returns>
	public static ChainableComparer<T> Then<T>(this ChainableComparer<T> chainable, Comparison<T> next,
		Direction sortDirection) =>
		chainable.Then(new ComparisonComparer<T>(next, sortDirection));
}