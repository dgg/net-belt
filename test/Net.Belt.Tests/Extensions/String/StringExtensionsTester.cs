using Net.Belt.Extensions.String;
using Net.Belt.Tests.Extensions.String.Support;
using Net.Belt.ValueObjects;

using NUnit.Framework.Internal;

using Testing.Commons;

using Iz = Net.Belt.Tests.Extensions.String.Support.Iz;

namespace Net.Belt.Tests.Extensions.String;

[TestFixture]
public class StringExtensionsTester
{
	#region emptiness

	[Test]
	[TestCase(null, true)]
	[TestCase("", true)]
	[TestCase("  ", true)]
	[TestCase("abc", false)]
	public void IsEmpty_Spec(string? input, bool isEmpty) =>
		Assert.That(input.IsEmpty, Is.EqualTo(isEmpty));

	[Test]
	[TestCase(null, false)]
	[TestCase("", false)]
	[TestCase("  ", false)]
	[TestCase("abc", true)]
	public void IsNotEmpty_Spec(string? input, bool isNotEmpty) =>
		Assert.That(input.IsNotEmpty, Is.EqualTo(isNotEmpty));

	[Test]
	[TestCase(null, "", Description = "empty")]
	[TestCase("", "", Description = "same")]
	[TestCase(" ", " ", Description = "same")]
	[TestCase("not empty", "not empty", Description = "same")]
	public void EmptyIfNull_Empty_Null(string? input, string? emptyIfNull) =>
		Assert.That(input.EmptyIfNull(), Is.EqualTo(emptyIfNull));
	
	#endregion
	
	#region Substr

	#region right

	[Test]
	[TestCase("lazy lazy fox jumped", 3, "ped", Description = "0..length --> last 3 chars substring")]
	[TestCase("lazy lazy fox jumped", 20, "lazy lazy fox jumped", Description = "length --> whole")]
	[TestCase("lazy lazy fox jumped", 30, null, Description = "> length --> empty substring")]
	[TestCase("lazy lazy fox jumped", 0, "", Description = "zero length --> empty")]
	[TestCase("lazy lazy fox jumped", -1, null, Description = "negative length --> empty substring")]
	[TestCase("", 1, null, Description = "> length --> empty substring")]
	[TestCase("", 0, "", Description = "length --> whole")]
	public void Right_Spec(string input, int length, string? substring)
	{
		Substring result = substring is null ? Substring.Empty : new Substring(substring);
		Assert.That(input.Substr.Right(length), Is.EqualTo(result).Using(Substring.Comparer));
	}
	
	[Test]
	[TestCase("", "lazy lazy fox jumped", null, Description = "not found -> empty substring")]
	[TestCase("", "", "", Description = "found at beginning --> whole")]
	[TestCase("lazy lazy fox jumped", "lazy lazy fox jumped", "", Description = "found at end --> empty")]
	[TestCase("lazy lazy fox jumped", "", "lazy lazy fox jumped", Description = "found at beginning --> whole")]
	[TestCase("lazy lazy fox jumped", "l", "azy lazy fox jumped", Description = "contained char --> right from first index")]
	[TestCase("lazy lazy fox jumped", "f", "ox jumped", Description = "only contained char --> right from index")]
	[TestCase("lazy lazy fox jumped", "azy", " lazy fox jumped", Description = "contained string --> right from first index + len")]
	[TestCase("lazy lazy fox jumped", "d", "", Description = "end char --> empty")]
	[TestCase("lazy lazy fox jumped", "ed", "", Description = "end substring --> empty")]
	[TestCase("lazy lazy fox jumped", "c", null, Description = "not found --> empty substring")]
	[TestCase("lazy lazy fox jumped", "lozy", null, Description = "not found --> empty substring")]
	[TestCase("DOMAIN\\username", "\\", "username")]
	public void RightFromFirst_Specification(string input, string substring, string? expected)
	{
		Substring result = expected is null ? Substring.Empty : new Substring(expected);
		Assert.That(input.Substr.RightFromFirst(substring), Is.EqualTo(result).Using(Substring.Comparer));
	}
	
	[Test]
	[TestCase("", "lazy lazy fox jumped", null, Description = "not found -> empty substring")]
	[TestCase("", "", "", Description = "found at beginning --> whole")]
	[TestCase("lazy lazy fox jumped", "lazy lazy fox jumped", "", Description = "found at end --> empty")]
	[TestCase("lazy lazy fox jumped", "", "", Description = "found at beginning --> empty")]
	[TestCase("lazy lazy fox jumped", "l", "azy fox jumped", Description = "contained char --> right from last index")]
	[TestCase("lazy lazy fox jumped", "f", "ox jumped", Description = "only contained char --> right from index")]
	[TestCase("lazy lazy fox jumped", "azy", " fox jumped", Description = "contained string --> right from last index + len")]
	[TestCase("lazy lazy fox jumped", "d", "", Description = "end char --> empty")]
	[TestCase("lazy lazy fox jumped", "ed", "", Description = "end substring --> empty")]
	[TestCase("lazy lazy fox jumped", "c", null, Description = "not found --> empty substring")]
	[TestCase("lazy lazy fox jumped", "lozy", null, Description = "not found --> empty substring")]
	[TestCase("DOMAIN\\username", "\\", "username")]
	public void RightFromLast_Specification(string input, string substring, string? expected)
	{
		Substring result = expected is null ? Substring.Empty : new Substring(expected);
		Assert.That(input.Substr.RightFromLast(substring), Is.EqualTo(result).Using(Substring.Comparer));
	}

	#endregion
	
	#region left
	
	[Test]
	[TestCase("lazy lazy fox jumped", 4, "lazy", Description = "first 4 chars")]
	[TestCase("lazy lazy fox jumped", 30, null, Description = "> length --> argument")]
	[TestCase("lazy lazy fox jumped", 0, "", Description = "zero length --> empty")]
	[TestCase("lazy lazy fox jumped", -1, null, Description = "negative length --> empty substring")]
	[TestCase("", 1, null, Description = "> length --> empty substring")]
	public void Left_Specification(string input, int length, string? expected)
	{
		Substring result = expected is null ? Substring.Empty : new Substring(expected);
		Assert.That(input.Substr.Left(length), Is.EqualTo(result).Using(Substring.Comparer));
	}
	
	[Test]
	[TestCase("", "lazy lazy fox jumped", null, Description = "not found --> empty substring")]
	[TestCase("", "", "", Description = "beginning --> empty")]
	[TestCase("lazy lazy fox jumped", "", "", Description = "beginning --> empty")]
	[TestCase("lazy lazy fox jumped", "l", "", Description = "beginning --> empty")]
	[TestCase("lazy lazy fox jumped", "f", "lazy lazy ", Description = "single contained char --> left from index")]
	[TestCase("lazy lazy fox jumped", "azy", "l", Description = "contained string --> left from index")]
	[TestCase("lazy lazy fox jumped", "d", "lazy lazy fox jumpe", Description = "last char --> remove last")]
	[TestCase("lazy lazy fox jumped", "ed", "lazy lazy fox jump", Description = "last substring --> remove last substring")]
	[TestCase("lazy lazy fox jumped", "c", null, Description = "not found char --> empty substring")]
	[TestCase("lazy lazy fox jumped", "lozy", null, Description = "not found string --> empty substring")]
	[TestCase("DOMAIN\\username", "\\", "DOMAIN")]
	public void LeftFromFirst_Specification(string input, string substring, string? expected)
	{
		Substring result = expected is null ? Substring.Empty : new Substring(expected);
		Assert.That(input.Substr.LeftFromFirst(substring), Is.EqualTo(result).Using(Substring.Comparer));
	}
	
	[TestCase("", "lazy lazy fox jumped", null, Description = "Snot found --> empty substring")]
	[TestCase("", "", "", Description = "beginning --> empty")]
	[TestCase("lazy lazy fox jumped", "", "lazy lazy fox jumped", Description = "beginning --> *")]
	[TestCase("lazy lazy fox jumped", "l", "lazy ", Description = "contained char --> left from last index")]
	[TestCase("lazy lazy fox jumped", "f", "lazy lazy ", Description = "single contained char -> left from index")]
	[TestCase("lazy lazy fox jumped", "azy", "lazy l", Description = "contained string --> left from index")]
	[TestCase("lazy lazy fox jumped", "d", "lazy lazy fox jumpe", Description = "last char --> remove last")]
	[TestCase("lazy lazy fox jumped", "ed", "lazy lazy fox jump", Description = "last substring --> remove lsat substring")]
	[TestCase("lazy lazy fox jumped", "c", null, Description = "not found --> empty substring")]
	[TestCase("lazy lazy fox jumped", "lozy", null, Description = "not found string --> empty substring")]
	[TestCase("DOMAIN\\username", "\\", "DOMAIN")]
	public void LeftFromLast_Specification(string input, string substring, string? expected)
	{
		Substring result = expected is null ? Substring.Empty : new Substring(expected);
		Assert.That(input.Substr.LeftFromLast(substring), Is.EqualTo(result).Using(Substring.Comparer));
	}
	
	#endregion
	
	#endregion
	
	#region Chunkify
	
	[Test]
	public void Chunkify_LengthLessThanChunk_OneChunk() =>
		Assert.That("123".Chunkify(5), Is.EqualTo(["123"]));
	
	[Test]
	public void Chunkify_LengthMultipleOfChunkSize_MultipleChunksOfEqualSize() =>
		Assert.That("123456789".Chunkify(3), Is.EqualTo(["123", "456", "789"]));
	
	[Test]
	public void Chunkify_LengthMultipleOfChunkSize_LastChunkIsSmaller() =>
		Assert.That("123456789".Chunkify(5), Is.EqualTo(["12345", "6789"]));
	
	[Test]
	public void Chunkify_Zero_ThrowsArgumentOutOfRangeException() =>
		Assert.That(() => "123456789".Chunkify(0).Iterate(), Throws.InstanceOf<ArgumentOutOfRangeException>());

	#endregion
	
	#region Insert

	[Test]
	public void Insert_EveryChar_SeparatorEveryOtherChar() =>
		Assert.That("input".Insert("-").Every(1), Is.EqualTo("i-n-p-u-t"));
	
	[Test]
	[TestCase("", "-", 1, "", Description = "empty string")]
	[TestCase("a", "-", 1, "a", Description = "single char")]
	[TestCase("ab", "-", 1, "a-b", Description = "two chars every 1")]
	[TestCase("abc", "-", 1, "a-b-c", Description = "three chars every 1")]
	[TestCase("abcd", "-", 2, "ab-cd", Description = "four chars every 2")]
	[TestCase("abcdefgh", "-", 3, "abc-def-gh", Description = "eight chars every 3")]
	[TestCase("12345", ":", 1, "1:2:3:4:5", Description = "numeric string with colon separator")]
	[TestCase("abc", "--", 1, "a--b--c", Description = "multi-char separator every 1")]
	[TestCase("abcd", ":::", 2, "ab:::cd", Description = "multi-char separator every 2")]
	public void Every_VariousLengthsAndIntervals(string input, string separator, int interval, string expected) =>
		Assert.That(input.Insert(separator).Every((ushort)interval), Is.EqualTo(expected));
	
	[Test]
	public void Insert_EmptySeparator_SameString() =>
		Assert.That("abc".Insert(string.Empty).Every(1), Is.EqualTo("abc"));
	
	[Test]
	public void Insert_BigInterval_SameString() =>
		Assert.That("abc".Insert(string.Empty).Every(10), Is.EqualTo("abc"));

	[Test]
	public void Insert_EveryZeroChars_Exception() =>
		Assert.That(() => "abc".Insert("-").Every(0), Throws.InstanceOf<ArgumentOutOfRangeException>());

	#endregion
	
	#region compression
	
	[Test]
	public void Compress_LongString_SmallerFootprint()
	{
		string longString = new ('a', 120);

		Assert.That(longString, Has.Length.EqualTo(120));

		Assert.That(longString.GZip.Compress(), Has.Length.LessThan(120));
	}
	
	[Test]
	public void Compress_Base64()
	{
		string longString = new ('a', 120);
		Assert.That(longString.GZip.Compress(), Iz.Base64());
	}

	[Test]
	public void Decompress_GetsOriginalString()
	{
		string original = new string('a', 120),
			compressed = original.GZip.Compress();
			
		Assert.That(compressed.GZip.Decompress(), Is.EqualTo(original));
	}
	
	#endregion
}