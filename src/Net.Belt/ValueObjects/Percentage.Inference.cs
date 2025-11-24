using System.Numerics;

namespace Net.Belt.ValueObjects;

/// <summary>
/// Leverages type inference for <see cref="Percentage{T}"/>.
/// </summary>
public static class Percentage
{
	/// <summary>
	/// Creates a new <see cref="Percentage{T}"/> instance from a given value.
	/// </summary>
	/// <typeparam name="T">The type of the numeric value, which must implement <see cref="IFloatingPoint{T}"/>.</typeparam>
	/// <param name="value">The numeric value representing the percentage (e.g., 0.50 for 50%).</param>
	/// <returns>A new <see cref="Percentage{T}"/> instance.</returns>
	public static Percentage<T> From<T>(T value) where T : IFloatingPoint<T> => new(value);

	/// <summary>
	/// Creates a new <see cref="Percentage{T}"/> instance from a fraction.
	/// </summary>
	/// <typeparam name="T">The type of the numeric value, which must implement <see cref="IFloatingPoint{T}"/>.</typeparam>
	/// <param name="fraction">The fraction value (e.g., 0.25 for 25%).</param>
	/// <returns>A new <see cref="Percentage{T}"/> instance.</returns>
	public static Percentage<T> FromFraction<T>(T fraction) where T : IFloatingPoint<T> =>
		Percentage<T>.FromFraction(fraction);

	/// <summary>
	/// Creates a new <see cref="Percentage{T}"/> instance from a given amount and a total amount.
	/// </summary>
	/// <typeparam name="T">The type of the numeric value for the percentage, which must implement <see cref="IFloatingPoint{T}"/>.</typeparam>
	/// <typeparam name="TAmount">The type of the amounts, which must implement <see cref="INumber{TAmount}"/>.</typeparam>
	/// <param name="given">The given amount.</param>
	/// <param name="total">The total amount.</param>
	/// <returns>A new <see cref="Percentage{T}"/> instance representing the percentage of <paramref name="given"/> out of <paramref name="total"/>.</returns>
	public static Percentage<T> FromAmounts<T, TAmount>(TAmount given, TAmount total)
		where T : IFloatingPoint<T>
		where TAmount : INumber<TAmount> =>
		Percentage<T>.FromAmounts(given, total);
}