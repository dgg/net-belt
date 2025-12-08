namespace Net.Belt.Extensions.String;

/// <summary>
/// Provides extension methods for <see cref="string"/> objects.
/// </summary>
public static class StringExtensions
{
	
	/// <summary>
	/// Provides extension methods for nullable <see cref="string"/> objects.
	/// </summary>
	/// <param name="s">The string instance to extend.</param>
	extension(string? s)
	{
		/// <summary>
		/// Gets a value indicating whether the string is <see langword="null"/>, empty, or consists only of white-space characters.
		/// </summary>
		public bool IsEmpty => string.IsNullOrWhiteSpace(s);
		
		/// <summary>
		/// Gets a value indicating whether the string is not <see langword="null"/>, empty, or consists only of white-space characters.
		/// </summary>
		public bool IsNotEmpty => !s.IsEmpty;
		
		/// <summary>
		/// Returns the string itself if it's not <see langword="null"/>, otherwise returns <see cref="string.Empty"/>.
		/// </summary>
		/// <returns>The original string if not <see langword="null"/>; otherwise, <see cref="string.Empty"/>.</returns>
		public string EmptyIfNull() => s ?? string.Empty;
	}

	/// <summary>
	/// Provides extension methods for non-nullable <see cref="string"/> objects.
	/// </summary>
	/// <param name="s">The string instance to extend.</param>
	extension(string s)
	{
		/// <summary>
		/// Groups substring extensions on the current string.
		/// </summary>
		public SubstrExtension Substr => new(s);
	}
}

