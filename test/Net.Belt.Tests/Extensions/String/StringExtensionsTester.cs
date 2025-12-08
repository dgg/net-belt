using Net.Belt.Extensions.String;
using Net.Belt.Tests.Extensions.String.Support;
using Net.Belt.ValueObjects;

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
	
}