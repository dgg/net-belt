using Dumpify;

using Net.Belt.ValueObjects;

using Testing.Commons.Time;

namespace Net.Belt.Tests.ValueObjects;
[TestFixture, Category("showcase"), Explicit("noisy")]
public class AgeShowcase
{
	[Test]
	public void Showcase()
	{
		new Age(11.March(1977), 1.December(2025))
			.Dump("-ties");

		new Age(7.November(2012), 14.November(2012))
			.Dump("one week old");
		
		DateOnly terminus = DateOnly.FromDateTime(DateTime.Now);
		DateOnly advent = DateOnly.FromDateTime(terminus.ToDateTime(TimeOnly.MinValue).AddYears(-300).AddDays(-15));
		var vampireAge = new Age(advent, terminus);
		vampireAge.ToString()
			.Dump("record-like");

		Age twoDaysIn77 = new (11.March(1977), 13.March(1977));
		Age twoDaysIn12 = new(27.November(2012), 29.November(2012));
		
		twoDaysIn12.Equals(twoDaysIn77)
			.Dump("not quite the same");
		(twoDaysIn77 != twoDaysIn12)
			.Dump("operators agree");

		twoDaysIn12.CompareTo(twoDaysIn77)
			.Dump("but they are very comparable");
		(twoDaysIn12 < vampireAge)
			.Dump("2d < 300+y");
	}
}