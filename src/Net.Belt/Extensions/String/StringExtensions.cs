using System.Text.RegularExpressions;

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

		/// <summary>
		/// Splits the string into chunks of the specified size.
		/// </summary>
		/// <param name="chunkSize">The maximum size of each chunk.</param>
		/// <returns>An enumerable of string chunks.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when <paramref name="chunkSize"/> is <c>0</c>.
		/// </exception>
		public IEnumerable<string> Chunkify(ushort chunkSize)
		{
			if (chunkSize == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "Chunk size cannot be zero.");
			}

			int offset = 0;
			while (offset < s.Length)
			{
				int size = Math.Min(chunkSize, s.Length - offset);
				yield return s.Substring(offset, size);
				offset += size;
			}
		}
		
		/// <summary>
		/// Provides extension methods for insertion operations.
		/// </summary>
		/// <param name="separator">The separator string to be inserted.</param>
		public InsertExtensions Insert(string separator) => new(s, separator);
		
		/// <summary>
		/// Provides extension methods for compression operations.
		/// </summary>
		public ZipExtensions GZip => new(s);
	}
}

