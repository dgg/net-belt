using Dumpify;

using Net.Belt.Extensions.String;

namespace Net.Belt.Tests.Extensions.String;

[TestFixture, Category("showcase")]
public class StringExtensionsShowcase
{
	[Test]
	public void Emptiness()
	{
		string? nil = null;
		"something".IsEmpty
			.Dump("not empty");
		nil.IsEmpty
			.Dump("very empty");
		string.Empty.IsEmpty
			.Dump("quite empty");
		"  ".IsEmpty
			.Dump("moderately empty");
		
		nil.IsNotEmpty
			.Dump("very empty");
		string.Empty.IsNotEmpty
			.Dump("quite empty");
		"  ".IsNotEmpty
			.Dump("moderately empty");

		"something".EmptyIfNull()
			.Dump("same");
		nil.EmptyIfNull()
			.Dump("null -> empty");
		string.Empty.EmptyIfNull()
			.Dump("still empty");
		"  ".EmptyIfNull().Length
			.Dump("spaces are not null");
	}

	[Test]
	public void Substringing()
	{
		string quijote = "En un lugar de La Mancha...";

		quijote.Substr.Left(2)
			.Dump("two chars from left");
		quijote.Substr.Left(40)
			.Dump("overflow");
		quijote.Substr.Left(0)
			.Dump("zero chars? always, but always empty");

		quijote.Substr.Right(3)
			.Dump("3 chars from right");
		quijote.Substr.Right(40)
			.Dump("overflow");
		quijote.Substr.Right(0)
			.Dump("zero chars? always, but always empty");
	}
	
	[Test]
	public void FromOccurrence()
	{
		string quijote = "En un lugar de La Mancha...";

		quijote.Substr.LeftFromFirst(" un")
			.Dump();
		quijote.Substr.LeftFromFirst("m")
			.Dump("not found");
		quijote.Substr.LeftFromFirst("e", StringComparison.OrdinalIgnoreCase)
			.Dump("beginning");
		quijote.Substr.LeftFromLast("e")
			.Dump();
		quijote.Substr.LeftFromLast("m")
			.Dump("not found");

		quijote.Substr.RightFromFirst(" UN", StringComparison.InvariantCultureIgnoreCase)
			.Dump("ignore casing");
		quijote.Substr.RightFromFirst("m")
			.Dump("not found");
		quijote.Substr.RightFromFirst("...")
			.Dump("end");
		quijote.Substr.RightFromLast("e")
			.Dump();
		quijote.Substr.RightFromLast("m")
			.Dump("not found");
	}

	[Test]
	public void Decomposition()
	{
		"1234567890".Chunkify(3)
			.Dump("four chunks");
		"1234567890".Chunkify(100)
			.Dump("one big chunk");
	}

	[Test]
	public void Interleave()
	{
		"input".Insert("-").Every(1)
			.Dump();
		"input".Insert(" ").Every(2)
			.Dump();
		"input".Insert("#").Every(100)
			.Dump("no separator");
	}
	
	[Test]
	public void Compress()
	{
		string l = "not so long string";

		string compressed = l.GZip.Compress()
			.Dump("useless for short strings");
		compressed.GZip.Decompress()
			.Dump();

		l = new string('a', 2000);
		compressed = l.GZip.Compress()
			.Dump("now we are talking");
		compressed.GZip.Decompress().Length
			.Dump(compressed.Length.ToString());
	}
	
	[Test]
	public void Concatenate()
	{
		"DOMAIN".IfNotThere.Append("\\")
			.Dump("postfixed");
		
		"USER_NAME".IfNotThere.Prepend("\\")
			.Dump("prefixed");

		"https://hostname.com/".IfNotThere.Append("/")
			.Dump("no need to append");
		
		"https://hostname.com/".IfNotThere.Prepend(Uri.UriSchemeHttps)
			.Dump("no need to prepend");
	}
}