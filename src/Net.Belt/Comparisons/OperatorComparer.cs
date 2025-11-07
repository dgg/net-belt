using System.Linq.Expressions;

namespace Net.Belt.Comparisons;

/// <summary>
/// Represents a comparer that uses operator-based comparisons to compare objects of the specified type.
/// This comparer facilitates comparisons by leveraging predefined relational operators
/// and supports defining an ordering direction (ascending or descending).
/// </summary>
/// <typeparam name="T">The type of objects to be compared. The specified type must support operator-based comparisons.</typeparam>
public class OperatorComparer<T>(Direction direction = Direction.Ascending) : ChainableComparer<T>(direction)
{
	private static class Operations
	{
		internal static readonly Func<T, T, bool> Gt, Lt;
		static Operations()
		{
			Type t = typeof(T);
			ParameterExpression param1 = Expression.Parameter(t, "x");
			ParameterExpression param2 = Expression.Parameter(t, "y");

			Gt = Expression.Lambda<Func<T, T, bool>>(
					Expression.GreaterThan(param1, param2), param1, param2)
				.Compile();

			Lt = Expression.Lambda<Func<T, T, bool>>(
					Expression.LessThan(param1, param2), param1, param2)
				.Compile();
		}
	}

	/// <summary>
	/// Compares two objects of type <typeparamref name="T"/> using greater than or less than operations.
	/// </summary>
	/// <param name="x">The first object to compare.</param>
	/// <param name="y">The second object to compare.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>:
	/// 1 if <paramref name="x"/> is greater than <paramref name="y"/>,
	/// -1 if <paramref name="x"/> is less than <paramref name="y"/>,
	/// and 0 if they are equal.
	/// </returns>
	protected override int DoCompare(T x, T y)
	{
		return OperatorComparer<T>.throwingMeaningfulException(() =>
		{
			if (Operations.Gt(x, y)) return 1;
			if (Operations.Lt(x, y)) return -1;
			return 0;
		});
			
	}

	private static int throwingMeaningfulException(Func<int> comparison)
	{
		try
		{
			return comparison();
		}
		catch (TypeInitializationException ex)
		{
			// throw the more expressive exception of the static constructor
			if (ex.InnerException != null) throw ex.InnerException;
			throw;
		}
	}
}