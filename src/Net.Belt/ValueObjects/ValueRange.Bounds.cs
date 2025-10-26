using Net.Belt.Extensions.Comparable;

namespace Net.Belt.ValueObjects;

/// <summary>
/// Represents a closed (inclusive) boundary point in a value range.
/// </summary>
/// <typeparam name="T">The type of the boundary value, which must implement IComparable{T}.</typeparam>
/// <param name="Value">The boundary value.</param>
/// <remarks>
/// A closed bound includes the boundary value itself in the range. For example, in the range [5, 10],
/// both 5 and 10 are included in the range.
/// </remarks>
internal readonly record struct Closed<T>(T Value) : IBound<T> where T : IComparable<T>
{
	/// <summary>
	/// Gets the string representation of this bound when used as a lower bound.
	/// </summary>
	/// <returns>A string in the format "[value" indicating a closed lower bound.</returns>
	public string Lower() => "[" + Value;

	/// <summary>
	/// Gets the string representation of this bound when used as an upper bound.
	/// </summary>
	/// <returns>A string in the format "value]" indicating a closed upper bound.</returns>
	public string Upper() => Value + "]";

	/// <summary>
	/// Determines whether the bound value is less than or equal to the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is less than or equal to the specified value; otherwise, false.</returns>
	/// <remarks>
	/// For a closed bound, this includes equality since the bound is inclusive.
	/// </remarks>
	public bool LessThan(T other) => Value.IsAtMost(other);

	/// <summary>
	/// Determines whether the bound value is greater than or equal to the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is greater than or equal to the specified value; otherwise, false.</returns>
	/// <remarks>
	/// For a closed bound, this includes equality since the bound is inclusive.
	/// </remarks>
	public bool MoreThan(T other) => Value.IsAtLeast(other);

	/// <summary>
	/// Generates a value for range iteration.
	/// </summary>
	/// <param name="nextGenerator">A function that generates the next value based on the current value.</param>
	/// <returns>The current boundary value, since a closed bound includes the boundary itself.</returns>
	/// <remarks>
	/// For closed bounds, the value itself is returned without applying the generator,
	/// as the boundary value is included in the range.
	/// </remarks>
	public T Generate(Func<T, T> nextGenerator) => Value;

	/// <summary>
	/// Converts the bound to an assertion string representation.
	/// </summary>
	/// <returns>A string in the format "value (inclusive)" for error messages and assertions.</returns>
	public string ToAssertion() => Value + " (inclusive)";

	/// <summary>
	/// Gets a value indicating whether this bound is closed (inclusive).
	/// </summary>
	/// <value>Always returns true for a closed bound.</value>
	public bool IsClosed => true;

	/// <summary>
	/// Determines whether this bound touches another bound (i.e., they share a common value).
	/// </summary>
	/// <param name="bound">The bound to check against.</param>
	/// <returns>true if both bounds are closed and have the same value; otherwise, false.</returns>
	/// <remarks>
	/// Two closed bounds touch only if they have the same value. An open bound never touches
	/// any other bound, as open bounds exclude their boundary values.
	/// </remarks>
	public bool Touches(IBound<T> bound) => bound.IsClosed && Value.Equals(bound.Value);

	/// <summary>
	/// Determines whether this bound is equal to another bound.
	/// </summary>
	/// <param name="other">The bound to compare with.</param>
	/// <returns>true if the other bound is also a Closed{T} with the same value; otherwise, false.</returns>
	public bool Equals(IBound<T>? other) => other is Closed<T> && EqualityComparer<T>.Default.Equals(Value, other.Value);
}

/// <summary>
/// Represents an open (exclusive) boundary point in a value range.
/// </summary>
/// <typeparam name="T">The type of the boundary value, which must implement IComparable{T}.</typeparam>
/// <param name="Value">The boundary value.</param>
/// <remarks>
/// An open bound excludes the boundary value itself from the range. For example, in the range (5, 10),
/// neither 5 nor 10 are included in the range; only values strictly between them are included.
/// </remarks>
internal readonly record struct Open<T>(T Value) : IBound<T> where T : IComparable<T>
{
	/// <summary>
	/// Gets the string representation of this bound when used as a lower bound.
	/// </summary>
	/// <returns>A string in the format "(value" indicating an open lower bound.</returns>
	public string Lower() => "(" + Value;

	/// <summary>
	/// Gets the string representation of this bound when used as an upper bound.
	/// </summary>
	/// <returns>A string in the format "value)" indicating an open upper bound.</returns>
	public string Upper() => Value + ")";

	/// <summary>
	/// Determines whether the bound value is strictly less than the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is strictly less than the specified value; otherwise, false.</returns>
	/// <remarks>
	/// For an open bound, this excludes equality since the bound is exclusive.
	/// </remarks>
	public bool LessThan(T other) => Value.IsLessThan(other);

	/// <summary>
	/// Determines whether the bound value is strictly greater than the specified value.
	/// </summary>
	/// <param name="other">The value to compare against.</param>
	/// <returns>true if the bound value is strictly greater than the specified value; otherwise, false.</returns>
	/// <remarks>
	/// For an open bound, this excludes equality since the bound is exclusive.
	/// </remarks>
	public bool MoreThan(T other) => Value.IsMoreThan(other);

	/// <summary>
	/// Generates a value for range iteration.
	/// </summary>
	/// <param name="nextGenerator">A function that generates the next value based on the current value.</param>
	/// <returns>The next value after the current boundary value, as computed by the generator function.</returns>
	/// <remarks>
	/// For open bounds, the generator is applied to produce the next value,
	/// since the boundary value itself is excluded from the range.
	/// </remarks>
	public T Generate(Func<T, T> nextGenerator) => nextGenerator(Value);

	/// <summary>
	/// Converts the bound to an assertion string representation.
	/// </summary>
	/// <returns>A string in the format "value (not inclusive)" for error messages and assertions.</returns>
	public string ToAssertion() => Value + " (not inclusive)";

	/// <summary>
	/// Gets a value indicating whether this bound is closed (inclusive).
	/// </summary>
	/// <value>Always returns false for an open bound.</value>
	public bool IsClosed => false;

	/// <summary>
	/// Determines whether this bound touches another bound (i.e., they share a common value).
	/// </summary>
	/// <param name="bound">The bound to check against.</param>
	/// <returns>Always returns false, as open bounds never touch other bounds.</returns>
	/// <remarks>
	/// Open bounds exclude their boundary values, so they cannot touch any other bound,
	/// even if they have the same value.
	/// </remarks>
	public bool Touches(IBound<T> bound) => false;

	/// <summary>
	/// Determines whether this bound is equal to another bound.
	/// </summary>
	/// <param name="other">The bound to compare with.</param>
	/// <returns>true if the other bound is also an Open{T} with the same value; otherwise, false.</returns>
	public bool Equals(IBound<T>? other) => other is Open<T> && EqualityComparer<T>.Default.Equals(Value, other.Value);
}

/// <summary>
/// Provides factory methods for creating open and closed bounds.
/// </summary>
public static class Bound
{
	/// <summary>
	/// Creates an open (exclusive) bound with the specified value.
	/// </summary>
	/// <typeparam name="T">The type of the boundary value, which must implement IComparable{T}.</typeparam>
	/// <param name="value">The boundary value to use.</param>
	/// <returns>An open bound that excludes the specified value from the range.</returns>
	/// <remarks>
	/// Open bounds are exclusive, meaning the boundary value itself is not included in the range.
	/// For example, (5, 10) represents values greater than 5 and less than 10.
	/// </remarks>
	public static IBound<T> Open<T>(T value) where T : IComparable<T> => new Open<T>(value);

	/// <summary>
	/// Creates a closed (inclusive) bound with the specified value.
	/// </summary>
	/// <typeparam name="T">The type of the boundary value, which must implement IComparable{T}.</typeparam>
	/// <param name="value">The boundary value to use.</param>
	/// <returns>A closed bound that includes the specified value in the range.</returns>
	/// <remarks>
	/// Closed bounds are inclusive, meaning the boundary value itself is included in the range.
	/// For example, [5, 10] represents values from 5 to 10, including both 5 and 10.
	/// </remarks>
	public static IBound<T> Closed<T>(T value) where T : IComparable<T> => new Closed<T>(value);
	
	internal static void AssertRestrictionArguments<T>(IBound<T> x, IBound<T> y) where T : IComparable<T>
	{
		if (!x.Value.IsEqualTo(y.Value))
		{
			throw new ArgumentException("Bound values need to be equal to check restrictiveness.", nameof(y));
		}
	}
}