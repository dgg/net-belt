namespace Net.Belt.Extensions.String;

/// <summary>
/// Provides extension methods for conditional string concatenation operations.
/// </summary>
/// <param name="s">The source string to perform conditional concatenation operations on.</param>
public readonly struct ConditionalConcatenationExtensions(string s)
{
	/// <summary>
	/// Conditionally prepends a prefix to the string if it doesn't already start with the specified prefix.
	/// </summary>
	/// <param name="prefix">The prefix to prepend if not already present.</param>
	/// <param name="comparison">The type of string comparison to use. Default is <see cref="StringComparison.Ordinal"/>.</param>
	/// <returns>
	/// The original string if it already starts with the specified prefix,
	/// otherwise a new string with the prefix prepended.
	/// </returns>
	public string Prepend(string prefix, StringComparison comparison = StringComparison.Ordinal) =>
		s.StartsWith(prefix, comparison) ? s : string.Concat(prefix, s);
	/// <summary>
	/// Conditionally appends a suffix to the string if it doesn't already end with the specified suffix.
	/// </summary>
	/// <param name="suffix">The suffix to append if not already present.</param>
	/// <param name="comparison">The type of string comparison to use. Default is <see cref="StringComparison.Ordinal"/>.</param>
	/// <returns>
	/// The original string if it already ends with the specified suffix,
	/// otherwise a new string with the suffix appended.
	/// </returns>
	public string Append(string suffix, StringComparison comparison = StringComparison.Ordinal) =>
		s.EndsWith(suffix, comparison) ? s : string.Concat(s, suffix);
}