namespace Net.Belt.Comparisons;

/// <summary>
/// A specialized implementation of <see cref="ChainableEqualizer{T}"/> that delegates equality
/// and hash code computation to the provided delegates for comparison and hashing respectively.
/// </summary>
/// <typeparam name="T">The type of objects to compare and compute hash codes for.</typeparam>
public class DelegatedEqualizer<T>(Func<T, T, bool> equals, Func<T, int> hasher) : ChainableEqualizer<T>
{
	/// <summary>
	/// A customizable equality comparer implementation that uses provided delegate functions
	/// for determining equality and generating hash codes.
	/// </summary>
	public DelegatedEqualizer(Comparison<T> comparison, Func<T, int> hasher)
		: this(new ComparisonComparer<T>(comparison), hasher)  { }

	/// <summary>
	/// A customizable equality comparer that uses provided delegate functions for checking equality
	/// and generating hash codes, allowing flexible and reusable comparison logic.
	/// </summary>
	public DelegatedEqualizer(IComparer<T> comparer, Func<T, int> hasher)
		: this((x, y) => comparer.Compare(x, y) == 0, hasher) { }

	/// <summary>
	/// Determines whether the specified objects are equal using the delegated equality function.
	/// </summary>
	/// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
	/// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
	/// <returns>Returns <c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
	protected override bool DoEquals(T x, T y)
	{
		return equals(x, y);
	}

	/// <summary>
	/// Computes the hash code for the given object by delegating the computation
	/// to the hash code function provided during the construction of the <see cref="DelegatedEqualizer{T}"/>.
	/// </summary>
	/// <param name="obj">The object for which the hash code is to be computed.</param>
	/// <returns>The computed hash code for the specified object.</returns>
	protected override int DoGetHashCode(T obj)
	{
		return hasher(obj);
	}
}

public static partial class ChainableExtensions
{
	/// <summary>
	/// Adds equality logic and hash code generation to the current chainable equality comparer.
	/// </summary>
	/// <typeparam name="T">The type of objects to compare and hash.</typeparam>
	/// <param name="chainable">The existing chainable equalizer to extend.</param>
	/// <param name="equals">A delegate representing equality logic between two objects of type <typeparamref name="T"/>.</param>
	/// <param name="hasher">A delegate that computes the hash code for an object of type <typeparamref name="T"/>.</param>
	/// <returns>A new <see cref="ChainableEqualizer{T}"/> instance with the additional equality and hash code logic applied.</returns>
	public static ChainableEqualizer<T> Then<T>(this ChainableEqualizer<T> chainable, Func<T, T, bool> equals,
		Func<T, int> hasher) =>
		chainable.Then(new DelegatedEqualizer<T>(equals, hasher));
}