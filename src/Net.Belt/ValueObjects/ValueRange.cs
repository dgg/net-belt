using System.Globalization;
using System.Linq.Expressions;

using Net.Belt.Extensions.Comparable;

namespace Net.Belt.ValueObjects;

/// <summary>
/// Represents an immutable value range with lower and upper bounds of a comparable type.
/// </summary>
/// <typeparam name="T">The type of values in the range. Must implement IComparable{T}.</typeparam>
/// <remarks>
/// This type is implemented as an immutable record for value semantics.
/// Both bounds are represented by the IBound{T} interface, allowing for different bound types
/// (e.g., inclusive or exclusive).
/// </remarks>
public readonly record struct ValueRange<T> : IFormattable where T : IComparable<T>
{
	/// <summary>
	/// Gets the lower bound of the range as a <see cref="Bound{T}"/> instance (e.g., open or closed).
	/// </summary>
	public Bound<T> LowerBound { get; }

	/// <summary>
	/// Gets the value of the lower bound.
	/// </summary>
	public T Lower => LowerBound.Value;

	/// <summary>
	/// Gets the upper bound of the range as a <see cref="Bound{T}"/> instance (e.g., open or closed).
	/// </summary>
	public Bound<T> UpperBound { get; }

	/// <summary>
	/// Gets the value of the upper bound.
	/// </summary>
	public T Upper => UpperBound.Value;

	/// <summary>
	/// Indicates whether the current range is empty, meaning it does not represent any valid values (Null Object).
	/// </summary>
	public bool IsEmpty { get; private init; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueRange{T}"/> with specified lower and upper bounds.
	/// </summary>
	/// <param name="lowerBound">The lower bound of the range.</param>
	/// <param name="upperBound">The upper bound of the range.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the lower bound is greater than the upper bound.</exception>
	public ValueRange(Bound<T> lowerBound, Bound<T> upperBound)
	{
		AssertBounds(lowerBound, upperBound);
		LowerBound = lowerBound;
		UpperBound = upperBound;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueRange{T}"/> class with the specified lower and upper bounds.
	/// </summary>
	/// <param name="lowerBound">The lower bound of the range.</param>
	/// <param name="upperBound">The upper bound of the range.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if the lower bound is greater than the upper bound.</exception>
	public ValueRange(T lowerBound, T upperBound)
	{
		AssertBounds(lowerBound, upperBound);

		LowerBound = Bound.Closed(lowerBound);
		UpperBound = Bound.Closed(upperBound);
	}

	/// <summary>
	/// Determines whether the specified <paramref name="item"/> is contained within this range,
	/// taking into account the openness/closedness of the bounds.
	/// </summary>
	/// <param name="item">The value to test.</param>
	/// <returns><see langword="true"/> if the value is within the range; otherwise, <see langword="false"/>.</returns>
	public bool Contains(T item) => LowerBound.LessThan(item) && UpperBound.MoreThan(item);

	/// <summary>
	/// Computes the intersection of the current value range with another specified value range.
	/// Returns a new value range representing the overlapping portion of the two ranges, if any.
	/// </summary>
	/// <param name="range">The value range to intersect with the current value range. Can be null.</param>
	/// <returns>
	/// A new <see cref="ValueRange{T}"/> representing the intersection of the two ranges.
	/// If no intersection exists or the provided range is null, returns the empty value range.
	/// </returns>
	public ValueRange<T> Intersect(ValueRange<T>? range)
	{
		ValueRange<T> intersection = Empty;

		if (range is { IsEmpty: false })
		{
			if (LowerBound.Touches(range.Value.UpperBound))
			{
				intersection = ValueRange.Degenerate(Lower);
			}
			else if (UpperBound.Touches(range.Value.LowerBound))
			{
				intersection = ValueRange.Degenerate(Upper);
			}
			else if (Lower.IsLessThan(range.Value.Upper) && Upper.IsMoreThan(range.Value.Lower))
			{
				Bound<T> lower = max(LowerBound, range.Value.LowerBound, (x, y) => x.MoreRestrictive(y)),
					upper = min(UpperBound, range.Value.UpperBound, (x, y) => x.MoreRestrictive(y));
				intersection = new ValueRange<T>(lower, upper);
			}
		}

		return intersection;
	}

	/// <summary>
	/// Returns a new <see cref="ValueRange{T}"/> that combines the current range with the specified range,
	/// resulting in a range that spans both ranges.
	/// </summary>
	/// <param name="range">The range to join with the current range. If null or empty, only the current range is returned.</param>
	/// <returns>A new <see cref="ValueRange{T}"/> representing the union of the two ranges. If the input range is null or empty, the current range is returned.</returns>
	public ValueRange<T> Join(ValueRange<T>? range)
	{
		if (!range.HasValue || range.Value.IsEmpty) return this;
		if (IsEmpty) return range.Value;

		Bound<T> lower = min(LowerBound, range.Value.LowerBound, (x, y) => x.LessRestrictive(y)),
			upper = max(UpperBound, range.Value.UpperBound, (x, y) => x.LessRestrictive(y));

		return new ValueRange<T>(lower, upper);
	}

	/// <summary>
	/// Determines whether the current range overlaps with the specified range.
	/// </summary>
	/// <param name="range">The range to check for overlap against the current range.</param>
	/// <returns>True if the ranges overlap; otherwise, false.</returns>
	public bool Overlaps(ValueRange<T>? range)
	{
		if (!range.HasValue || range.Value.IsEmpty) return false;

		bool overlaps = LowerBound.Touches(range.Value.UpperBound) ||
			UpperBound.Touches(range.Value.LowerBound) ||
			(Lower.IsLessThan(range.Value.Upper) && Upper.IsMoreThan(range.Value.Lower));

		return overlaps;
	}

	/// <summary>
	/// Limits the given value to the range's lower bound if it is less than the lower bound.
	/// </summary>
	/// <param name="value">The value to be limited.</param>
	/// <returns>The adjusted value if less than the lower bound; otherwise, returns the original value.</returns>
	public T LimitLower(T value) => IsEmpty ? value : limit(value, Lower, value);

	/// <summary>
	/// Returns the specified value limited to the upper bound of the range.
	/// </summary>
	/// <param name="value">The input value to be compared against the upper bound.</param>
	/// <returns>The input value if it is less than or equal to the upper bound; otherwise, the upper bound.</returns>
	public T LimitUpper(T value) => IsEmpty ? value : limit(value, value, Upper);

	/// <summary>
	/// Restricts a value to be within the defined lower and upper bounds of the range.
	/// </summary>
	/// <param name="value">The value to be restricted.</param>
	/// <returns>
	/// The value adjusted to be within the bounds, returning the lower bound if the value is less, or the upper bound if it exceeds.
	/// </returns>
	public T Limit(T value) => IsEmpty ? value : limit(value, Lower, Upper);

	/// <summary>
	/// Generates a sequence of values within the range using the specified generator function.
	/// </summary>
	/// <param name="nextGenerator">A function that generates the next value in the sequence based on the current value.</param>
	/// <returns>A sequence of values contained within the range.</returns>
	/// <exception cref="ArgumentException">Thrown if the generator function does not produce incrementing values.</exception>
	public IEnumerable<T> Generate(Func<T, T> nextGenerator)
	{
		T numberInRange = LowerBound.Generate(nextGenerator);
		while (UpperBound.MoreThan(numberInRange))
		{
			yield return numberInRange;
			T next = nextGenerator(numberInRange);
			if (next.IsAtMost(numberInRange))
				throw new ArgumentException("The generator must generate incrementing values", nameof(nextGenerator));
			numberInRange = next;
		}
	}

	/// <summary>
	/// Generates a sequence of values within the range, starting from the lower bound and incrementing by the specified value.
	/// </summary>
	/// <param name="increment">The value by which to increment each step in the sequence.</param>
	/// <returns>An enumerable sequence of values within the range.</returns>
	public IEnumerable<T> Generate(T increment)
	{
		Func<T, T> generator = initNextGenerator(increment);
		return Generate(generator);
	}

	#region bound checking

	/// <summary>
	/// Returns true if the raw values form a valid range (i.e., <paramref name="lowerBound"/> is less than
	/// or equal to <paramref name="upperBound"/>).
	/// </summary>
	public static bool CheckBounds(T lowerBound, T upperBound) => lowerBound.IsAtMost(upperBound);

	/// <summary>
	/// Returns true if the provided bounds form a valid range (i.e., the lower bound is strictly less than
	/// the upper bound value, honoring bound semantics).
	/// </summary>
	public static bool CheckBounds(Bound<T> lowerBound, Bound<T> upperBound) => lowerBound.LessThan(upperBound.Value);

	/// <summary>
	/// Ensures the specified bounds form a valid range; throws if they do not.
	/// </summary>
	/// <param name="lowerBound">The lower bound to validate.</param>
	/// <param name="upperBound">The upper bound to validate.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the bounds are invalid.</exception>
	public static void AssertBounds(Bound<T> lowerBound, Bound<T> upperBound)
	{
		if (!CheckBounds(lowerBound, upperBound))
		{
			throw exception(lowerBound.Value, upperBound.Value);
		}
	}

	/// <summary>
	/// Ensures that the specified lower and upper bounds form a valid range, throwing an exception if they do not.
	/// </summary>
	/// <param name="lowerBound">The lower bound to validate.</param>
	/// <param name="upperBound">The upper bound to validate.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the lower bound is greater than the upper bound, indicating an invalid range.
	/// </exception>
	public static void AssertBounds(T lowerBound, T upperBound)
	{
		if (!CheckBounds(lowerBound, upperBound))
		{
			throw exception(lowerBound, upperBound);
		}
	}

	/// <summary>
	/// Creates an <see cref="ArgumentOutOfRangeException"/> describing an invalid range definition.
	/// </summary>
	/// <param name="lowerBound">The invalid lower-bound value.</param>
	/// <param name="upperBound">The invalid upper-bound value.</param>
	/// <returns>An exception that explains the invalid range.</returns>
	private static ArgumentOutOfRangeException exception(T lowerBound, T upperBound)
	{
		string? lowerRepresentation = lowerBound is IFormattable upperFormattable ? upperFormattable.ToString(null, CultureInfo.InvariantCulture) : lowerBound.ToString();
		string? upperRepresentation = upperBound is IFormattable lowerFormattable ? lowerFormattable.ToString(null, CultureInfo.InvariantCulture) : upperBound.ToString();
		string message = string.Create(CultureInfo.InvariantCulture,
			$"The lower bound of the range ('{lowerRepresentation}') must not be greater than upper bound ('{upperRepresentation}').");

		return new ArgumentOutOfRangeException(nameof(upperBound), upperBound, message);
	}

	/// <summary>
	/// Validates that the specified <paramref name="value"/> lies within this range.
	/// </summary>
	/// <param name="paramName">The name of the argument being validated.</param>
	/// <param name="value">The value to validate.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not contained within the range.</exception>
	public void AssertArgument(string paramName, T value)
	{
		if (!Contains(value))
		{
			string message =
				$"The value must be between {LowerBound.ToAssertion()} and {UpperBound.ToAssertion()}. That is, contained within {this}.";
			throw new ArgumentOutOfRangeException(paramName, value, message);
		}
	}

	/// <summary>
	/// Validates that all values in <paramref name="values"/> lie within this range.
	/// </summary>
	/// <param name="paramName">The name of the argument being validated.</param>
	/// <param name="values">The sequence of values to validate. Must not be <see langword="null"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when any value is not contained within the range.</exception>
	public void AssertArgument(string paramName, IEnumerable<T> values)
	{
		if (values == null) throw new ArgumentNullException(nameof(values));

		foreach (var value in values)
		{
			AssertArgument(paramName, value);
		}
	}

	#endregion

	#region Empty Range

	/// <summary>
	/// Gets an empty instance of <see cref="ValueRange{T}"/> for a specific type.
	/// Represents a range that contains no elements.
	/// </summary>
	public static ValueRange<T> Empty { get; } = new() { IsEmpty = true };

	#endregion

	/// <remarks>
	/// <c>..</c> separator used instead of standard <c>,</c> to avoid confusion with rational ranges.
	/// <para><c>[ ]</c> used for closed bounds.</para>
	/// <para><c>( )</c> used for open bounds, as it is clearer than inverted brackets <c>] [</c>.</para>
	/// </remarks>
	public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

	/// <inheritdoc />
	public string ToString(string? format, IFormatProvider? formatProvider) =>
		$"{LowerBound.Lower(format, formatProvider)}..{UpperBound.Upper(format, formatProvider)}";

	private static Bound<T> min(Bound<T> x, Bound<T> y, Func<Bound<T>, Bound<T>, Bound<T>> equalSelection)
	{
		Bound<T> min;
		if (x.Value.IsEqualTo(y.Value))
		{
			min = equalSelection(x, y);
		}
		else
		{
			min = x.Value.IsLessThan(y.Value) ? x : y;
		}

		return min;
	}

	private static Bound<T> max(Bound<T> x, Bound<T> y, Func<Bound<T>, Bound<T>, Bound<T>> equalSelection)
	{
		Bound<T> max;
		if (x.Value.IsEqualTo(y.Value))
		{
			max = equalSelection(x, y);
		}
		else
		{
			max = x.Value.IsMoreThan(y.Value) ? x : y;
		}

		return max;
	}

	private static T limit(T value, T lowerBound, T upperBound)
	{
		T result = value;
		if (value.IsMoreThan(upperBound)) result = upperBound;
		if (value.IsLessThan(lowerBound)) result = lowerBound;
		return result;
	}

	private static Func<T, T> initNextGenerator(T step)
	{
		ParameterExpression current = Expression.Parameter(typeof(T), "current");
		Expression<Func<T, T>> nextExpr = Expression.Lambda<Func<T, T>>(
			Expression.Add(
				current,
				Expression.Constant(step)),
			current);
		return nextExpr.Compile();
	}
}