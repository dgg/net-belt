namespace Net.Belt.ValueObjects;

using System.Numerics;

/// <summary>
/// Represents a percentage value, generic to allow both <see cref="double"/> and <see cref="decimal"/> types.
/// This record struct provides functionality to represent a percentage and its fractional equivalent,
/// and is formattable using the <see cref="IFormattable"/> interface.
/// </summary>
/// <typeparam name="T">The numeric type of the percentage, must implement <see cref="IFloatingPoint{TSelf}"/>.</typeparam>
/// <param name="Value">The percentage value (e.g., 50 for 50%).</param>
public readonly record struct Percentage<T>(T Value) : IFormattable, IComparable<Percentage<T>>
	where T : IFloatingPoint<T>
{
	/// <summary>
	/// Gets the fractional value of the percentage (e.g., 0.50 for 50%).
	/// </summary>
	public T Fraction { get; } = Value / T.CreateChecked(100);

	/// <summary>
	/// Creates a new <see cref="Percentage{T}"/> instance from a fractional value.
	/// </summary>
	/// <param name="fraction">The fractional value (e.g., 0.50 for 50%).</param>
	/// <returns>A new <see cref="Percentage{T}"/> instance.</returns>
	public static Percentage<T> FromFraction(T fraction) => new(fraction * T.CreateChecked(100));

	/// <summary>
	/// Creates a <see cref="Percentage{T}"/> from two amounts, representing the <paramref name="given"/> amount out of the <paramref name="total"/> amount.
	/// </summary>
	/// <typeparam name="TAmount">The numeric type of the amounts, must implement <see cref="INumber{TSelf}"/>.</typeparam>
	/// <param name="given">The given amount.</param>
	/// <param name="total">The total amount. Must not be zero.</param>
	/// <returns>A new <see cref="Percentage{T}"/> instance.</returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="total"/> is zero.</exception>
	public static Percentage<T> FromAmounts<TAmount>(TAmount given, TAmount total) where TAmount : INumber<TAmount>
	{
		assertNonZero(total);
		return new Percentage<T>(T.CreateChecked(given) / T.CreateChecked(total) * T.CreateChecked(100));
	}

	private static void assertNonZero<TAmount>(TAmount total) where TAmount : INumber<TAmount>
	{
		if (total.Equals(TAmount.Zero))
		{
			throw new ArgumentException("Amount cannot be zero when calculating percentage.", nameof(total));
		}
	}

	/// <summary>
	/// Creates a <see cref="Percentage{T}"/> representing the difference between two amounts relative to the <paramref name="total"/> amount.
	/// </summary>
	/// <typeparam name="TAmount">The numeric type of the amounts, must implement <see cref="INumber{TSelf}"/>.</typeparam>
	/// <param name="total">The total amount. Must not be zero.</param>
	/// <param name="given">The amount to subtract from the total.</param>
	/// <returns>A new <see cref="Percentage{T}"/> instance representing the percentage difference.</returns>
	/// <exception cref="ArgumentException">Thrown if <paramref name="total"/> is zero.</exception>
	public static Percentage<T> FromDifference<TAmount>(TAmount total, TAmount given) where TAmount : INumber<TAmount>
	{
		assertNonZero(total);
		return new Percentage<T>(T.CreateChecked(total - given) / T.CreateChecked(total) * T.CreateChecked(100));
	}

	#region IComparable<>

	/// <inheritdoc/>
	public int CompareTo(Percentage<T> other) => Value.CompareTo(other.Value);

	/// <summary>
	/// Compares two values to determine which is less.
	/// </summary>
	/// <param name="left">The <see cref="Percentage{T}"/> value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The <see cref="Percentage{T}"/> value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator <(Percentage<T> left, Percentage<T> right) => left.Value < right.Value;

	/// <summary>
	/// Compares two values to determine which is less or equal.
	/// </summary>
	/// <param name="left">The <see cref="Percentage{T}"/> value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The <see cref="Percentage{T}"/> value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator <=(Percentage<T> left, Percentage<T> right) => left.Value <= right.Value;

	/// <summary>
	/// Compares two values to determine which is greater.
	/// </summary>
	/// <param name="left">The <see cref="Percentage{T}"/> value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The <see cref="Percentage{T}"/> value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator >(Percentage<T> left, Percentage<T> right) => left.Value > right.Value;

	/// <summary>
	/// Compares two values to determine which is greater or equal.
	/// </summary>
	/// <param name="left">The <see cref="Percentage{T}"/> value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The <see cref="Percentage{T}"/> value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static bool operator >=(Percentage<T> left, Percentage<T> right) => left.Value >=right.Value;

	#endregion

	#region IFormattable

	/// <inheritdoc/>
	public override string ToString() => ToString(null, null);

	/// <summary>
	/// Returns a string representation of the <see cref="Percentage{T}"/> value using the invariant culture.
	/// </summary>
	/// <param name="format">The format specifier.</param>
	/// <returns>A string representation of the value.</returns>
	public string ToString(string? format) => ToString(format, null);

	/// <summary>
	/// Returns a string representation of the <see cref="Percentage{T}"/> value using the specified format and culture.
	/// </summary>
	/// <param name="format">The format specifier to use. "P" for percentage (e.g., "50.00%"), "F" for fraction (e.g., "0.50").</param>
	/// <param name="formatProvider">The format provider to use.</param>
	/// <returns>A string representation of the value.</returns>
	/// <exception cref="FormatException">Thrown if the format string is invalid.</exception>
	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		switch (format)
		{
			case null:
			case { } when string.IsNullOrWhiteSpace(format): return Fraction.ToString("P", formatProvider);

			case { } when format.StartsWith("P"): return Fraction.ToString(format, formatProvider);
			case { } when format.StartsWith("F"): return Value.ToString(format, formatProvider);
			default: return Value.ToString(format, formatProvider);
		}
	}

	#endregion

	/// <summary>
	/// Applies this percentage to the specified <paramref name="given"/> value and returns the resulting value.
	/// </summary>
	/// <param name="given">The value to which the percentage will be applied.</param>
	/// <returns>The resulting value after applying the percentage.</returns>
	public T Apply(T given) => Fraction * given;

	
	/// <summary>
	/// Deducts this percentage from the specified <paramref name="amountIncludingPercentage"/> value and returns the base value.
	/// </summary>
	/// <param name="amountIncludingPercentage">The amount that includes the percentage to be deducted.</param>
	/// <returns>The base value after deducting the percentage.</returns>
	/// <remarks>
	/// This method calculates the original value before the percentage was applied.
	/// For example, if an item costs 120 and includes a 20% tax (this percentage),
	/// calling <c>.DeductFrom(120)</c> will return 100.
	/// </remarks>
	public T DeductFrom(T amountIncludingPercentage) => amountIncludingPercentage / (T.CreateChecked(1) + Fraction);
}