using Dumpify;

using Net.Belt.ValueObjects;

namespace Net.Belt.Tests.ValueObjects;
[TestFixture, Category("showcase"), Explicit("noisy")]
public class HexFigureShowcase
{
	[Test]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059")]
	public void Showcase()
	{
		var one = new HexFigure(1)
		.Dump("from numbers");

		var twelve = new HexFigure('C')
			.Dump("from characters");

		twelve.ToString()
			.Dump("character");

		twelve.ToString("N", null)
			.Dump("numeric");

		one.Equals(twelve)
			.Dump("not quite the same");
		(twelve != one)
			.Dump("operators agree");

		one.CompareTo(twelve)
			.Dump("a little bit negative");
		(one > HexFigure.Zero)
			.Dump("1 > Zero");

		HexFigure.Parse("11", null)
			.Dump("B");

		HexFigure.TryParse("zero", null, out HexFigure _)
			.Dump("no luck");
		
		HexFigure.TryParse("a", null, out HexFigure parsed)
			.Dump("luckier");
		parsed.Dump("F");
	}
}