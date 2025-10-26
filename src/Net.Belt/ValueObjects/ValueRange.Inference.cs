namespace Net.Belt.ValueObjects;

/// <summary>
/// Leverages type inference for <see cref="ValueRange{T}"/>.
/// </summary>
public static class ValueRange
{
	#region creation

	/// <summary>
	/// Creates a closed range: both bounds contained in the range.
	/// </summary>
	public static ValueRange<T> New<T>(T lowerBound, T upperBound) where T : IComparable<T> =>
		Closed(lowerBound, upperBound);

	/// <summary>
	/// Creates a closed range: both bounds contained in the range.
	/// </summary>
	public static ValueRange<T> Closed<T>(T lowerBound, T upperbound) where T : IComparable<T> =>
		new(Bound.Closed(lowerBound), Bound.Closed(upperbound));

	/// <summary>
	/// Creates an open range: neither of bounds contained in the range.
	/// </summary>
	public static ValueRange<T> Open<T>(T lowerBound, T upperbound) where T : IComparable<T> =>
		new(Bound.Open(lowerBound), Bound.Open(upperbound));

	/// <summary>
	/// Creates a half-open range: only the lower bound is contained in the range.
	/// </summary>
	/// <remarks>Closed at the beginning, open at the end.</remarks>
	public static ValueRange<T> HalfOpen<T>(T lowerBound, T upperbound) where T : IComparable<T> =>
		new(Bound.Closed(lowerBound), Bound.Open(upperbound));

	/// <summary>
	/// Creates a half-closed range: only the upper bound is contained in the range.
	/// </summary>
	/// <remarks>Open at the beginning, closed at the end.</remarks>
	public static ValueRange<T> HalfClosed<T>(T lowerBound, T upperbound) where T : IComparable<T> =>
		new(Bound.Open(lowerBound), Bound.Closed(upperbound));


	/// <summary>
	/// Creates a closed range with a single value
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	public static ValueRange<T> Degenerate<T>(T value) where T : IComparable<T> => Closed(value, value);

	/// <summary>
	/// Returns an empty range of a specified type.
	/// </summary>
	/// <typeparam name="T">The type of values the range supports. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <returns>A <see cref="ValueRange{T}"/> representing an empty range.</returns>
	public static ValueRange<T> Empty<T>() where T : IComparable<T> => ValueRange<T>.Empty;

	#endregion

	#region bound checking

	/// <summary>
	/// Validates whether the specified bounds form a valid range.
	/// </summary>
	/// <param name="lowerBound">The lower bound of the range.</param>
	/// <param name="upperBound">The upper bound of the range.</param>
	/// <typeparam name="T">The type of the bounds, which must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <returns>Returns <c>true</c> if the lower bound is less than or equal to the upper bound; otherwise, returns <c>false</c>.</returns>
	public static bool CheckBounds<T>(T lowerBound, T upperBound) where T : IComparable<T> =>
		ValueRange<T>.CheckBounds(lowerBound, upperBound);

	/// <summary>
	/// Ensures that the specified lower and upper bounds form a valid range, throwing an exception if they do not.
	/// </summary>
	/// <param name="lowerBound">The inclusive lower bound to validate.</param>
	/// <param name="upperBound">The inclusive upper bound to validate.</param>
	/// <typeparam name="T">The type of the bounds, constrained to types implementing <see cref="IComparable{T}"/>.</typeparam>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if the lower bound is greater than the upper bound, indicating an invalid range.
	/// </exception>
	public static void AssertBounds<T>(T lowerBound, T upperBound) where T : IComparable<T> =>
		ValueRange<T>.AssertBounds(lowerBound, upperBound);

	#endregion

	#region string generation

	/// <summary>
	/// Generates a new string by incrementing the last alphanumeric character in the input string,
	/// propagating the increment when necessary, and appending remaining non-alphanumeric characters as is.
	/// </summary>
	/// <param name="s">The input string to process and generate a new string from.</param>
	/// <returns>A new string with the last alphanumeric character incremented and other changes applied.</returns>
	public static string StringGenerator(string s)
	{
		int lastAlphaNumeric = -1;
		for (int i = s.Length - 1; i >= 0 && lastAlphaNumeric == -1; i--)
		{
			if (char.IsLetterOrDigit(s[i])) lastAlphaNumeric = i;
		}

		if (lastAlphaNumeric == s.Length - 1 || lastAlphaNumeric == -1) return succ(s, s.Length);
		return succ(s, lastAlphaNumeric + 1) + s[(lastAlphaNumeric + 1)..];
	}

	private static string succ(string val, int length)
	{
		char lastChar = val[length - 1];
		return lastChar switch
		{
			'9' => ((length > 1) ? succ(val, length - 1) : "1") + '0',
			'z' => ((length > 1) ? succ(val, length - 1) : "a") + 'a',
			'Z' => ((length > 1) ? succ(val, length - 1) : "A") + 'A',
			_ => val[..(length - 1)] + (char)(lastChar + 1)
		};
	}

	#endregion
}