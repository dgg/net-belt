using System.Text;

namespace Net.Belt.Extensions.String;

/// <summary>
/// Provides extension methods for inserting separators into strings.
/// </summary>
/// <param name="s">The string to insert separators into.</param>
/// <param name="separator">The separator string to insert.</param>
public readonly struct InsertExtensions(string s, string separator)
{
	/// <summary>
	/// Inserts the separator into the string every specified number of characters.
	/// </summary>
	/// <param name="characters">The number of characters between each separator insertion.</param>
	/// <returns>A new string with the separator inserted every specified number of characters.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="characters"/> is <c>0</c>.</exception>
	public string Every(ushort characters)
	{
		if (characters == 0)
		{
			throw new ArgumentOutOfRangeException(nameof(characters), characters, "Character interval cannot be zero.");
		}

		if (s.Length == 0)
		{
			return s;
		}

		int separatorCount = (s.Length - 1) / characters;
		int capacity = s.Length + (separatorCount * separator.Length);
		var result = new StringBuilder(capacity);
		for (int i = 0; i < s.Length; i++)
		{
			if (i > 0 && i % characters == 0)
			{
				result.Append(separator);
			}

			result.Append(s[i]);
		}

		return result.ToString();
	}
}