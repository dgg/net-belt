namespace Net.Belt.Comparisons;

/// <summary>
/// A comparer that reverses the comparison outcome of an inner comparer to enable reverse sorting.
/// </summary>
/// <typeparam name="T">The type of objects to be compared.</typeparam>
/// <remarks>
/// This comparer is useful when you want to sort in the opposite order of the given inner comparer.
/// It supports customizing the sort direction.
/// </remarks>
public class ReversedComparer<T>(IComparer<T> inner, Direction direction = Direction.Ascending)
	: ChainableComparer<T>(direction)
{
	/// <summary>
	/// Performs the comparison of two objects by reversing the result of the inner comparer.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of x and y:
	/// Less than zero if x is greater than y,
	/// zero if x equals y,
	/// greater than zero if x is less than y.
	/// </returns>
	protected override int DoCompare(T x, T y) => inner.Compare(y, x);
}