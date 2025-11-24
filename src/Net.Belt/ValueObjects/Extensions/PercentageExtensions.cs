using System.Numerics;

namespace Net.Belt.ValueObjects.Extensions;

/// <summary>
/// Provides extension methods for creating <see cref="Percentage{T}"/> instances from numeric values.
/// </summary>
public static class PercentageExtensions
{
	/// <summary>
	/// Converts a numeric value into a <see cref="Percentage{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">The type of the numeric value, which must implement <see cref="IFloatingPoint{T}"/>.</typeparam>
	/// <param name="value">The numeric value to convert to a percentage.</param>
	/// <returns>A new <see cref="Percentage{T}"/> instance representing the given <paramref name="value"/>.</returns>
	public static Percentage<T> Percent<T>(this T value) where T : IFloatingPoint<T> => new(value);
}