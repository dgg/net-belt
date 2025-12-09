using System.IO.Compression;
using System.Text;

namespace Net.Belt.Extensions.String;

/// <summary>
/// Provides extension methods for compressing and decompressing strings using GZip.
/// </summary>
/// <param name="s">The string to be compressed or decompressed.</param>
public readonly struct ZipExtensions(string s)
{
	/// <summary>
	/// Compresses the string provided to this <see cref="ZipExtensions"/> instance using GZip and encodes the result as a Base64 string.
	/// </summary>
	/// <returns>
	/// A Base64-encoded string representing the compressed input string.
	/// </returns>
	public string Compress()
	{
		byte[] buffer = Encoding.UTF8.GetBytes(s);

		using var memoryStream = new MemoryStream();
		using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
		{
			gZipStream.Write(buffer, 0, buffer.Length);
		}

		memoryStream.Position = 0;

		var compressedData = new byte[memoryStream.Length];
		memoryStream.ReadExactly(compressedData, 0, compressedData.Length);

		var gZipBuffer = new byte[compressedData.Length + 4];
		Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
		Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);

		return Convert.ToBase64String(gZipBuffer);
	}

	/// <summary>
	/// Decompresses the Base64-encoded, GZip-compressed string provided to this <see cref="ZipExtensions"/> instance.
	/// </summary>
	/// <returns>
	/// The original uncompressed string.
	/// </returns>
	public string Decompress()
	{
		byte[] gZipBuffer = Convert.FromBase64String(s);
		using var memoryStream = new MemoryStream();
		int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
		memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

		var buffer = new byte[dataLength];

		memoryStream.Position = 0;
		using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
		{
			gZipStream.ReadExactly(buffer, 0, buffer.Length);
		}

		return Encoding.UTF8.GetString(buffer);
	}
}