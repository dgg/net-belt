using Net.Belt.ValueObjects;

namespace Net.Belt.Extensions.String;

/// <summary>
/// Provides a set of extension methods for extracting substrings from a <see cref="string"/>.
/// </summary>
/// <param name="s">The string instance on which to perform substring operations.</param>
public readonly struct SubstrExtension(string s)
{
	#region Right

	/// <summary>
	/// Extracts a substring of a specified <paramref name="length"/> from the right side of the string.
	/// </summary>
	/// <param name="length">The number of characters to extract from the right.</param>
	/// <returns>A <see cref="Substring"/> containing the specified number of characters from the right of the string,
	/// or <see cref="Substring.Empty"/> if <paramref name="length"/> is outside the string.</returns>
	public Substring Right(int length) =>
		length switch
		{
			< 0 => Substring.Empty,
			0 => new Substring(string.Empty),
			_ => length > s.Length ? Substring.Empty : new Substring(s[^length..])
		};

	/// <summary>
	/// Extracts the substring to the right of the first occurrence of a specified <paramref name="substring"/>.
	/// </summary>
	/// <param name="substring">The string to seek.</param>
	/// <param name="comparison">One of the enumeration values that specifies the rules for the search.</param>
	/// <returns>A <see cref="Substring"/> containing the characters to the right of the first occurrence of <paramref name="substring"/>,
	/// or <see cref="Substring.Empty"/> if <paramref name="substring"/> is not found.</returns>
	public Substring RightFromFirst(string substring, StringComparison comparison = StringComparison.Ordinal)
	{
		int index = s.IndexOf(substring, comparison);
		int indexOfSubstringEnd = index < 0 ? index : index + substring.Length;
		return indexOfSubstringEnd switch
		{
			< 0 => Substring.Empty,
			0 => new Substring(s),
			_ => new Substring(s[^(s.Length - indexOfSubstringEnd)..])
		};
	}

	/// <summary>
	/// Extracts the substring to the right of the last occurrence of a specified <paramref name="substring"/>.
	/// </summary>
	/// <param name="substring">The string to seek.</param>
	/// <param name="comparison">One of the enumeration values that specifies the rules for the search.</param>
	/// <returns>A <see cref="Substring"/> containing the characters to the right of the last occurrence of <paramref name="substring"/>,
	/// or <see cref="Substring.Empty"/> if <paramref name="substring"/> is not found.</returns>
	public Substring RightFromLast(string substring, StringComparison comparison = StringComparison.Ordinal)
	{
		int index = s.LastIndexOf(substring, comparison);
		int indexOfSubstringEnd = index < 0 ? index : index + substring.Length;
		return indexOfSubstringEnd switch
		{
			< 0 => Substring.Empty,
			0 => new Substring(s),
			_ => new Substring(s[^(s.Length - indexOfSubstringEnd)..])
		};
	}

	#endregion

	#region Left

	/// <summary>
	/// Extracts a substring of a specified <paramref name="length"/> from the left side of the string.
	/// </summary>
	/// <param name="length">The number of characters to extract from the left.</param>
	/// <returns>A <see cref="Substring"/> containing the specified number of characters from the left of the string,
	/// or <see cref="Substring.Empty"/> if <paramref name="length"/> is outside the string.</returns>
	public Substring Left(int length) =>
		length switch
		{
			< 0 => Substring.Empty,
			0 => new Substring(string.Empty),
			_ => length > s.Length ? Substring.Empty : new Substring(s[..length])
		};

	/// <summary>
	/// Extracts the substring to the left of the first occurrence of a specified <paramref name="substring"/>.
	/// </summary>
	/// <param name="substring">The string to seek.</param>
	/// <param name="comparison">One of the enumeration values that specifies the rules for the search.</param>
	/// <returns>A <see cref="Substring"/> containing the characters to the left of the first occurrence of <paramref name="substring"/>,
	/// or <see cref="Substring.Empty"/> if <paramref name="substring"/> is not found.</returns>
	public Substring LeftFromFirst(string substring, StringComparison comparison = StringComparison.Ordinal)
	{
		int index = s.IndexOf(substring, comparison);
		return index < 0 ? Substring.Empty : new Substring(s[..index]);
	}
	
	/// <summary>
	/// Extracts the substring to the left of the last occurrence of a specified <paramref name="substring"/>.
	/// </summary>
	/// <param name="substring">The string to seek.</param>
	/// <param name="comparison">One of the enumeration values that specifies the rules for the search.</param>
	/// <returns>A <see cref="Substring"/> containing the characters to the left of the last occurrence of <paramref name="substring"/>,
	/// or <see cref="Substring.Empty"/> if <paramref name="substring"/> is not found.</returns>
	public Substring LeftFromLast(string substring, StringComparison comparison = StringComparison.Ordinal)
	{
		int index = s.LastIndexOf(substring, comparison);
		return index < 0 ? Substring.Empty : new Substring(s[..index]);
	}
	#endregion
}