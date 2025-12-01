using System.Diagnostics.CodeAnalysis;

using Dumpify;

using Net.Belt.Tests.ValueObjects.Support;
using Net.Belt.ValueObjects;

using Testing.Commons.Time;

namespace Net.Belt.Tests.ValueObjects;

[TestFixture, Category("showcase"), Explicit("noisy")]
public class ValueRangeShowcase
{
	[Test]
	public void Creation()
	{
		// closed by default
		var closed = new ValueRange<int>(2, 4)
			.Dump("closed by default");

		// type-inference factories
		ValueRange<string> factoryMade = ValueRange.New("A", "Z")
			.Dump("type-inference factories");

		// half-open using bounds
		var halfOpen = new ValueRange<int>(Bound.Closed(2), Bound<int>.Open(4))
			.Dump("half-open using bounds");

		// half-closed using a shortcut
		ValueRange<char> halfClosed = ValueRange.HalfClosed('a', 'z')
			.Dump("half-closed using a shortcut");

		// a point becomes a degenerated range
		ValueRange<DateTime> degenerate = ValueRange.Degenerate(DateTime.UtcNow)
			.Dump("a point becomes a degenerated range");

		// null-object is "less than" degenerated
		var empty = ValueRange.Empty<TimeSpan>()
			.Dump("null-object is 'less than' degenerated");
		
		// order can be checked before creation
		bool @false = ValueRange.CheckBounds("Z", "A")
			.Dump("order can be checked before creation");

		// ArgumentOutOfRangeException
		// ValueRange.AssertBounds(12m, 1m);
		// ValueRange.New("Z", "A");
	}
	
	[Test]
	public void Containment()
	{
		// well-contained
		ValueRange.Closed(2, 4).Contains(3)
			.Dump("well-contained");

		var halfOpen = ValueRange.HalfOpen(2, 4)
			.Dump();

		// lower is contained
		bool contained = halfOpen.Contains(2)
			.Dump("lower is contained");

		// upper is not
		bool notContained = halfOpen.Contains(4)
			.Dump("upper is not");
		
		// empty contains nothing
		bool @false = ValueRange.Empty<byte>().Contains(0)
			.Dump("empty contains nothing");
	}
	
	[Test]
	public void Set_Like()
	{
		// clean overlapping
		bool overlaps = ValueRange.Closed(2, 4).Overlaps(ValueRange.Closed(1, 3))
			.Dump("clean overlapping");

		// bound overlapping
		ValueRange.HalfOpen(2, 4).Overlaps(ValueRange.HalfClosed(1, 2))
			.Dump("bound overlapping");

		// no overlapping when bounds not there
		ValueRange.HalfOpen(2, 4).Overlaps(ValueRange.HalfOpen(1, 2))
			.Dump("no overlapping when bounds not there");

		// range generation by intersection
		ValueRange.Open(2, 4).Intersect(ValueRange.HalfOpen(-1, 3))
			.Dump("range generation by intersection");

		// empty intersection is empty
		ValueRange.Open(2, 4).Intersect(ValueRange.Empty<int>())
			.Dump("empty intersection is empty");

		// join overlapping
		ValueRange.Closed(2, 4).Join(ValueRange.Closed(1, 3))
			.Dump("join overlapping");

		// join bound overlapping
		ValueRange.HalfOpen(2, 4).Dump().Join(ValueRange.HalfClosed(1, 2).Dump())
			.Dump("join bound overlapping");

		// disjointed union
		ValueRange.Open(2, 4).Join(ValueRange.Closed(7, 9))
			.Dump("disjointed union");

		// empty is identity for .Join()
		ValueRange.Empty<char>().Join(ValueRange.Degenerate('a'))
			.Dump("Empty is identity for .Join()");
	}

	[Test]
	public void Limit()
	{
		// within limits, returns self
		ValueRange.Closed('a', 'z').Limit('m')
			.Dump("within limits, returns self");

		// outside Limits, returns lower
		ValueRange.Closed("2", "4").Limit("1")
			.Dump("outside Limits, returns lower");

		// outside Limits, returns upper, regardless of containment
		ValueRange.Open('2', '4').Limit('7')
			.Dump("outside limits returns upper, regardless of containment");

		// upper limits can be obviated
		ValueRange.Open(2, 4).LimitLower(7)
			.Dump("upper limits can be obviated");

		// lower limits can be obviated
		ValueRange.Open(2, 4).LimitUpper(1)
			.Dump("lower limits can be obviated");

		// empty is very lenient
		ValueRange.Empty<double>().Limit(double.NegativeInfinity)
			.Dump("empty is very lenient");
	}

	[Test]
	public void Generation()
	{
		// generate elements inside range
		ValueRange.Closed('a', 'z').Generate(c => Convert.ToChar(c + 1))
			.ToArray().Dump("generate elements inside range");

		// bounds nature is taken into account
		ValueRange.Open("0", "50").Generate(s => (byte.Parse(s) + 10).ToString())
			.ToArray().Dump("bounds nature is taken into account");

		// strings have special generators, a-la Ruby
		ValueRange.Closed("koala", "koale").Generate(ValueRange.StringGenerator)
			.ToArray().Dump("strings have special generators, a-la Ruby");

		// addition operands can be inferred
		ValueRange.Open(0m, 10m).Generate(2)
			.ToArray().Dump("addition operands can be inferred");

		// inference leads to surprises: InvalidOperationException
		// ValueRange.Open("1", "10").Generate("1");
	}

	[Test]
	public void Equality()
	{
		// same bounds with same nature
		bool @true = new ValueRange<int>(2, 4).Equals(ValueRange.Closed(2, 4))
			.Dump("same bounds with same nature");

		// same bounds with different nature
		bool @false = ValueRange.New(2, 4).Equals(ValueRange.Open(2, 4))
			.Dump("same bounds with different nature");
		
		// equal is the same for record structs
		(ValueRange.Closed('a', 'z') == ValueRange.Closed('a', 'z'))
			.Dump("Equal is the same for record structs");
		
		// type matters
		ValueRange.Closed<byte>(2, 4).Equals(ValueRange.Closed(2ul, 4ul))
			.Dump("type matters");

		// empty is an "only child"
		ValueRange.Empty<TimeSpan>().Equals(ValueRange.Degenerate(TimeSpan.Zero))
			.Dump("empty is an 'only child'");

		// but not so unique
		(ValueRange.Empty<TimeSpan>() == ValueRange.Empty<TimeSpan>())
			.Dump("but not so unique");
	}
}