using Net.Belt.Extensions.Comparable;

namespace Net.Belt.ValueObjects;

/// <summary>
/// Represents a boundary point in a value range.
/// </summary>
/// <typeparam name="T">The type of the boundary value, which must implement IComparable{T}.</typeparam>
/// <param name="Value">The boundary value.</param>
/// <param name="IsClosed">A value indicating whether this bound is closed (inclusive) or open (exclusive).</param>
/// <remarks>
/// A closed (inclusive) bound includes the boundary value itself in the range. For example, in the range [5, 10],
/// both 5 and 10 are included in the range.
/// <para>An open (exclusive) bound excludes the boundary value itself from the range. For example, in the range (5, 10),
/// neither 5 nor 10 are included in the range; only values strictly between them are included.</para>
/// </remarks>
public readonly record struct Bound<T>(T Value, bool IsClosed) where T : IComparable<T>
{
	/// <summary>
	/// Creates a new open (exclusive) boundary point with the specified value.
	/// </summary>
	/// <param name="value">The value of the boundary point.</param>
	/// <returns>A new <see cref="Bound{T}"/> instance with the given value and an open boundary.</returns>
	public static Bound<T> Open(T value) => new(value, false);

	/// <summary>
	/// Creates a new closed (inclusive) boundary point with the specified value.
	/// </summary>
	/// <param name="value">The value of the boundary point.</param>
	/// <returns>A new <see cref="Bound{T}"/> instance with the given value and a closed boundary.</returns>
	public static Bound<T> Closed(T value) => new(value, true);
	
	/// <summary>
	/// Gets the string representation of this bound when used as a lower bound.
	/// </summary>
	/// <returns>A string in the format "[value" indicating a closed lower bound or "(value" indicating an open lower bound.</returns>
	internal string Lower() => IsClosed ? "[" + Value : "(" + Value;

	/// <summary>
	/// Gets the string representation of this bound when used as an upper bound.
	/// </summary>
	/// <returns>A string in the format "value]" indicating a closed upper bound or "value)" indicating an open upper bound.</returns>
	internal string Upper() => IsClosed ? Value + "]" : Value + ")";
	
	/// <summary>
	/// Determines whether the bound value is less than the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is less than the specified value; otherwise, false.</returns>
	/// <remarks>
	/// For a closed bound, this includes equality since the bound is inclusive.
	/// <para>For an open bound, this excludes equality since the bound is exclusive.</para>
	/// </remarks>
	internal bool LessThan(T other) => IsClosed ? Value.IsAtMost(other) : Value.IsLessThan(other);
	
	/// <summary>
	/// Determines whether the bound value is more than the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is more than the specified value; otherwise, false.</returns>
	/// <remarks>
	/// For a closed bound, this includes equality since the bound is inclusive.
	/// <para>For an open bound, this excludes equality since the bound is exclusive.</para>
	/// </remarks>
	internal bool MoreThan(T other) => IsClosed ? Value.IsAtLeast(other) : Value.IsMoreThan(other);
	
	/// <summary>
	/// Converts the bound to an assertion string representation.
	/// </summary>
	/// <returns>A string in the format "value (inclusive/not inclusive)" for error messages and assertions.</returns>
	internal string ToAssertion() => $"{Value} {(IsClosed ? "(inclusive)" : "(not inclusive)")}";

	/// <summary>
	/// Determines whether this bound touches another bound (i.e., they share a common value).
	/// </summary>
	/// <param name="bound">The bound to check against.</param>
	/// <returns>true if the bounds touch; otherwise, false.</returns>
	/// <remarks>
	/// Two closed bounds touch only if they have the same value. An open bound never touches
	/// any other bound, as open bounds exclude their boundary values.
	/// <para>Open bounds exclude their boundary values, so they cannot touch any other bound,
	/// even if they have the same value.</para>
	/// </remarks>
	internal bool Touches(Bound<T> bound) => IsClosed && (bound.IsClosed && Value.Equals(bound.Value));
	
	/// <summary>
	/// Generates a value for range iteration.
	/// </summary>
	/// <param name="nextGenerator">A function that generates the next value based on the current value.</param>
	/// <returns>The generated value.</returns>
	/// <remarks>
	/// For closed bounds, the value itself is returned without applying the generator,
	/// as the boundary value is included in the range.
	/// <para>For open bounds, the generator is applied to produce the next value,
	/// since the boundary value itself is excluded from the range.</para>
	/// </remarks>
	internal T Generate(Func<T, T> nextGenerator) => IsClosed ? Value: nextGenerator(Value);
	
	/// <summary>
	/// Determines the less restrictive boundary between this instance and the specified boundary.
	/// </summary>
	/// <param name="y">The boundary to compare against.</param>
	/// <returns>The less restrictive boundary. If this instance is closed, it is returned. Otherwise, the specified boundary is returned if it is closed; otherwise, this instance is returned.</returns>
	internal Bound<T> LessRestrictive(Bound<T> y)
	{
		assertRestrictionArguments(y);
		if (IsClosed) return this;
		return y.IsClosed ? y : this;
	}

	/// <summary>
	/// Determines the more restrictive boundary between the current instance and the specified boundary.
	/// </summary>
	/// <param name="y">The boundary to compare against.</param>
	/// <returns>The more restrictive boundary between the current instance and the specified boundary.</returns>
	internal Bound<T> MoreRestrictive(Bound<T> y)
	{
		assertRestrictionArguments(y);
		if (IsClosed) return y;
		return y.IsClosed ? this : y;
	}
	
	private void assertRestrictionArguments(Bound<T> y)
	{
		if (!Value.IsEqualTo(y.Value))
		{
			throw new ArgumentException("Bound values need to be equal to check restrictiveness.", nameof(y));
		}
	}
}
