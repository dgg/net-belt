namespace Net.Belt.ValueObjects;

/// <summary>
/// Represents a boundary point in a range of comparable values.
/// </summary>
/// <typeparam name="T">The type of the boundary value, which must implement IComparable{T}.</typeparam>
public interface IBound<T> : IEquatable<IBound<T>> where T : IComparable<T>
{
	/// <summary>
	/// Gets the actual boundary value.
	/// </summary>
	T Value { get; }

	/// <summary>
	/// Gets the string representation of this bound when used as a lower bound.
	/// </summary>
	/// <returns>A string representing the lower-bound notation.</returns>
	string Lower();

	/// <summary>
	/// Gets the string representation of this bound when used as an upper bound.
	/// </summary>
	/// <returns>A string representing the upper-bound notation.</returns>
	string Upper();

	/// <summary>
	/// Determines whether the bound value is less than the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is less than the specified value; otherwise, false.</returns>
	bool LessThan(T other);

	/// <summary>
	/// Determines whether the bound value is more than the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is more than the specified value; otherwise, false.</returns>
	bool MoreThan(T other);

	/// <summary>
	/// Converts the bound to an assertion string representation.
	/// </summary>
	/// <returns>A string representing the bound as an assertion.</returns>
	string ToAssertion();

	/// <summary>
	/// Generates a new value based on the current bound value using the specified generator function.
	/// </summary>
	/// <param name="nextGenerator">A function that generates the next value based on the current value.</param>
	/// <returns>The generated value.</returns>
	T Generate(Func<T, T> nextGenerator);

	/// <summary>
	/// Gets a value indicating whether this bound is closed (inclusive) or open (exclusive).
	/// </summary>
	bool IsClosed { get; }

	/// <summary>
	/// Determines whether this bound touches another bound (i.e., they share a common value).
	/// </summary>
	/// <param name="bound">The bound to check against.</param>
	/// <returns>true if the bounds touch; otherwise, false.</returns>
	bool Touches(IBound<T> bound);

	/// <summary>
	/// Determines the less restrictive boundary between this instance and the specified boundary.
	/// </summary>
	/// <param name="y">The boundary to compare against.</param>
	/// <returns>The less restrictive boundary. If this instance is closed, it is returned. Otherwise, the specified boundary is returned if it is closed; otherwise, this instance is returned.</returns>
	IBound<T> LessRestrictive(IBound<T> y)
	{
		Bound.AssertRestrictionArguments(this, y);
		if (IsClosed) return this;
		return y.IsClosed ? y : this;
	}

	/// <summary>
	/// Determines the more restrictive boundary between the current instance and the specified boundary.
	/// </summary>
	/// <param name="y">The boundary to compare with the current instance.</param>
	/// <returns>The more restrictive boundary between the current instance and the specified boundary.</returns>
	IBound<T> MoreRestrictive(IBound<T> y)
	{
		Bound.AssertRestrictionArguments(this, y);
		if (IsClosed) return y;
		return y.IsClosed ? this : y;
	}
}