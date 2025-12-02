using System.Globalization;
using Dumpify;
using Net.Belt.ValueObjects;
using Net.Belt.ValueObjects.Extensions;

namespace Net.Belt.Tests.ValueObjects;

[TestFixture, Category("showcase"), Explicit("noisy")]
public class PercentageShowcase
{
	[Test]
	public void PercentageCreation()
	{
		new Percentage<double>(60d)
			.Dump("floating point 60%");
		Percentage.FromFraction(.6m)
			.Dump("fixed point 60%");

		Percentage<double>.FromAmounts(60L, 75L)
			.Dump("80% from two amounts");
		Percentage.FromAmounts<decimal, ushort>(75, 60)
			.Dump("125% from two amounts");

		Percentage<decimal>.FromDifference(20u, 10u)
			.Dump("fifty percent bigger");
		Percentage<double>.FromDifference(10L, 20L)
			.Dump("twice as small");
	}

	[Test]
	public void Operations()
	{
		50m.Percent().Apply(100m)
			.Dump("50% discount");
		
		Percentage<decimal> tax = 20m.Percent(); // 20%
		decimal amountIncludingTax = 120m;
		tax.DeductFrom(amountIncludingTax)
			.Dump("one hundred");
	}
	
	[Test]
	public void Representation()
	{
		33.343d.Percent().ToString()
			.Dump("current culture-dependent default");
		
		33.343d.Percent().ToString(null, CultureInfo.InvariantCulture)
			.Dump("current culture-independent default");
		
		33.343d.Percent().ToString("P3", CultureInfo.InvariantCulture)
			.Dump("current culture-independent custom format");
		
		33.343d.Percent().ToString("+000.#####", CultureInfo.InvariantCulture)
			.Dump("current culture-independent custom format");

		var formatter = new NumberFormatInfo { PercentSymbol = "lol!" };
		33.343d.Percent().ToString(null, formatter)
			.Dump("custom formatter");
	}
}