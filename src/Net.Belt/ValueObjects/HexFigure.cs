using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Net.Belt.ValueObjects;

/// <summary>
/// Represents a single hexadecimal digit, providing both its numeric value and its uppercase character representation.
/// </summary>
public readonly struct HexFigure : IEquatable<HexFigure>,
	IComparable, IComparable<HexFigure>, IComparisonOperators<HexFigure, HexFigure, bool>,
	IParsable<HexFigure>, ISpanParsable<HexFigure>
{
	/// <summary>
	/// Gets the numeric value of the hexadecimal digit (0-15).
	/// </summary>
	public byte Numeric { get; } = 0;

	// nullable backing field to define a non-default "default" value
	private readonly char? _char;
	/// <summary>
	/// Gets the character representation of the hexadecimal digit in uppercase (0-9, A-F).
	/// </summary>
	public char Character => _char.GetValueOrDefault('0');

	/// <summary>
	/// Initializes a new instance of the <see cref="HexFigure"/> struct with the specified value.
	/// </summary>
	/// <param name="numeric">The numeric value of the hexadecimal digit (between 0 and 15).</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="numeric"/> is not a valid hexadecimal value.</exception>
	public HexFigure(byte numeric)
	{
		assertInRange(numeric);

		Numeric = numeric;
		int value = Numeric < 10 ? ('0' + Numeric) : ('A' + Numeric - 10);
		_char = Convert.ToChar(value);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HexFigure"/> struct with the specified hexadecimal character.
	/// </summary>
	/// <param name="character">A character representing a hexadecimal digit (0-9, A-F, a-f).</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="character"/> is not a valid hexadecimal character.</exception>
	public HexFigure(char character)
	{
		assertInRange(character);

		Numeric = byte.Parse(new ReadOnlySpan<char>(ref character), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		_char = char.ToUpperInvariant(character);
	}

	/// <summary>
	/// Gets a <see cref="HexFigure"/> instance representing the hexadecimal digit zero.
	/// </summary>
	public static HexFigure Zero { get; } = new HexFigure();

	/// <summary>
	/// Gets a <see cref="HexFigure"/> instance representing the minimum hexadecimal digit.
	/// </summary>
	public static HexFigure Min { get; } = Zero;

	/// <summary>
	/// Gets a <see cref="HexFigure"/> instance representing the maximum hexadecimal digit.
	/// </summary>
	public static HexFigure Max { get; } = new HexFigure(15);

	/// <summary>
	/// Gets a tuple containing both the numeric value and character representation of this hexadecimal digit.
	/// </summary>
	/// <returns>A tuple with the numeric value and the uppercase character representation.</returns>
	public (byte numeric, char character) Value => (Numeric, Character);

	/// <summary>
	/// Deconstructs the hexadecimal digit into its numeric and character components.
	/// </summary>
	/// <param name="numeric">The numeric value of the hexadecimal digit.</param>
	/// <param name="character">The uppercase character representation.</param>
	public void Deconstruct(out byte numeric, out char character)
	{
		numeric = Numeric;
		character = Character;
	}

	/// <inheritdoc/>
	/// <returns>A single hexadecimal character in uppercase.</returns>
	public override string ToString() => Character.ToString(CultureInfo.InvariantCulture);

	/// <inheritdoc/>
	public bool Equals(HexFigure other) => Numeric == other.Numeric;

	/// <inheritdoc/>
	public override bool Equals(object? obj) => obj is HexFigure other && Equals(other);

	/// <inheritdoc/>
	public override int GetHashCode() => Numeric.GetHashCode();

	/// <inheritdoc/>
	public static bool operator ==(HexFigure left, HexFigure right) => left.Equals(right);

	/// <inheritdoc/>
	public static bool operator !=(HexFigure left, HexFigure right) => !left.Equals(right);

	/// <inheritdoc/>
	public int CompareTo(HexFigure other) => Numeric.CompareTo(other.Numeric);

	/// <inheritdoc/>
	public int CompareTo(object? obj) =>
	obj switch
	{
		null => 1,
		HexFigure other => CompareTo(other),
		_ => throw new ArgumentException($"Object must be of type '{nameof(HexFigure)}'.", nameof(obj))
	};

	/// <inheritdoc />
	public static bool operator <(HexFigure left, HexFigure right) => left.Numeric < right.Numeric;

	/// <inheritdoc />
	public static bool operator <=(HexFigure left, HexFigure right) => left.Numeric <= right.Numeric;

	/// <inheritdoc />
	public static bool operator >(HexFigure left, HexFigure right) => left.Numeric > right.Numeric;

	/// <inheritdoc />
	public static bool operator >=(HexFigure left, HexFigure right) => left.Numeric >= right.Numeric;

	/// <inheritdoc/>
	public static HexFigure Parse(string s, IFormatProvider? provider)
	{
		ArgumentNullException.ThrowIfNull(s);
		return Parse(s.AsSpan(), provider);
	}

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out HexFigure result)
	{
		if (s is null)
		{
			result = default;
			return false;
		}
		return TryParse(s.AsSpan(), provider, out result);
	}

	/// <inheritdoc/>
	public static HexFigure Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		if (s.Length != 1)
		{
			throw new FormatException("Input must be a single hexadecimal character.");
		}

		if (!checkInRange(s[0]))
		{
			throw new FormatException("Input character is not a valid hexadecimal character.");
		}

		return new HexFigure(s[0]);
	}

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out HexFigure result)
	{
		result = default;
		if (s.Length != 1) return false;

		char c = s[0];
		if (!checkInRange(c)) return false;

		result = new HexFigure(c);
		return true;
	}
	private static bool checkInRange(byte numeric) => numeric <= 15;
	private static bool checkInRange(char character) => char.IsAsciiHexDigit(character);

	private static void assertInRange(byte numeric)
	{
		if (!checkInRange(numeric))
		{
			throw new ArgumentOutOfRangeException(nameof(numeric), numeric, "Hexadecimal digits must be within [0..15].");
		}
	}

	private static void assertInRange(char character)
	{
		if (!checkInRange(character))
		{
			throw new ArgumentOutOfRangeException(nameof(character), character, "Hexadecimal characters must be within ['0'..'9'] ∪ ['A'..'F'] ∪ ['a'..'f'].");
		}
	}
}
