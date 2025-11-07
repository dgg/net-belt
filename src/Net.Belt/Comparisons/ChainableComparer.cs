using System.Diagnostics.CodeAnalysis;

namespace Net.Belt.Comparisons;

/// <summary>
/// Provides a base implementation of a chainable comparer for comparing objects of a specified type.
/// This comparer allows you to chain multiple comparers in sequence to perform complex comparison logic.
/// </summary>
/// <typeparam name="T">The type of objects to be compared.</typeparam>
public abstract class ChainableComparer<T>(Direction sortDirection = Direction.Ascending) : IComparer<T>
{
	/// <summary>
	/// Gets the sorting direction, which determines whether the comparison is performed in ascending or descending order.
	/// </summary>
	/// <remarks>
	/// The sorting direction is specified as a <see cref="Direction"/> enumeration value and is used to control the order in which objects are compared.
	/// </remarks>
	public Direction SortDirection => sortDirection;

	/// <summary>
	/// Performs the comparison logic for the specified objects of type <typeparamref name="T"/>.
	/// This method must be overridden in derived classes to define the custom comparison logic.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>:
	/// <list type="bullet">
	/// <item>A value less than zero indicates that <paramref name="x"/> is less than <paramref name="y"/>.</item>
	/// <item>Zero indicates that <paramref name="x"/> equals <paramref name="y"/>.</item>
	/// <item>A value greater than zero indicates that <paramref name="x"/> is greater than <paramref name="y"/>.</item>
	/// </list>
	/// </returns>
	protected abstract int DoCompare(T x, T y);

	/// <inheritdoc />
	public int Compare(T? x, T? y)
	{
		int? shortCircuit = handleNulls(x, y);
		if (shortCircuit.HasValue) return shortCircuit.Value;

		// they are not null, so we can compare them
		int result = DoCompare(x!, y!);
		if (needsToEvaluateNext(result))
		{
			result = _nextComparer.Compare(x, y);
		}

		if (sortDirection == Direction.Descending) invert(ref result);

		return result;
	}

	private static int? handleNulls(T? x, T? y)
	{
		int? shortCircuit = null;
		if (!typeof(T).IsValueType)
		{
			if (x == null)
			{
				shortCircuit = y == null ? 0 : -1;
			}
			else if (y == null)
			{
				shortCircuit = 1;
			}
		}

		return shortCircuit;
	}

	[MemberNotNullWhen(true, nameof(_nextComparer))]
	private bool needsToEvaluateNext(int ret) => ret == 0 && _nextComparer != null;

	private static void invert(ref int result) => result *= -1;

	private ChainableComparer<T>? _nextComparer;

	private void chain(ChainableComparer<T> lastComparer)
	{
		if (_nextComparer == null)
		{
			_nextComparer = lastComparer;
		}
		else
		{
			_nextComparer.chain(lastComparer);
		}
	}

	private ChainableComparer<T>? _lastComparer;

	/// <summary>
	/// Appends the specified chainable comparer to the current comparer, enabling the chaining of comparison operations.
	/// </summary>
	/// <param name="comparer">The subsequent comparer to append to the current comparer.</param>
	/// <returns>
	/// The current chainable comparer instance with the specified comparer appended, allowing for further chaining of comparers.
	/// </returns>
	public ChainableComparer<T> Then(ChainableComparer<T> comparer)
	{
		if (_nextComparer == null)
		{
			_nextComparer = comparer;
		}
		else
		{
			_lastComparer?.chain(comparer);
		}

		_lastComparer = comparer;
		return this;
	}
}