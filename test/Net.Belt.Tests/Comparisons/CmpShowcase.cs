using Dumpify;

using Net.Belt.Comparisons;

namespace Net.Belt.Tests.Comparisons;

[TestFixture, Category("showcase"), Explicit("noisy")]
public class CmpShowcase
{
	internal record Subject(string S, int I, decimal D)
	{
		public static Subject a = new ("A", 4, 7.61m );
		public static Subject b = new  ("b",1, 3.00m );
		public static Subject c = new  ("C",3, 7.60m );
		public static Subject d = new  ("D",2, 3.00m );
		public static Subject e = new  ("E",5, 6.40m );

		public static List<Subject> All = [e, d, b, a, c];

		public static bool operator >(Subject x, Subject y) => x.D > y.D;

		public static bool operator <(Subject x, Subject y) => x.D < y.D;
	}
	
	internal class MyComparer : ChainableComparer<Subject>
	{
		// custom comparison of Subject
		protected override int DoCompare(Subject x, Subject y) => 0;
	}

	[Test]
	public void ChainableComparers()
	{
		Subject.All.Sort(Cmp<Subject>.By(p => p.I));
		Subject.All.Dump("sorted by I");
	
		Subject.All.Sort(Cmp<Subject>.By(p => p.I, Direction.Descending));
		Subject.All.Dump("sorted by I descending");

		// chaining comparers by direction
		Subject.All
			.OrderBy(s => s,
				Cmp<Subject>
					.By(p => p.D)
					.Then(p => p.S, Direction.Descending))
			.Dump("order by properties D and reverse S");

		// use the Comparison delegate
		var adaptComparison = Cmp<Subject>.By(
			(x, y) => StringComparer.Ordinal.Compare(x.S, y.S)) as ComparisonComparer<Subject>;
		Subject.All.Sort(adaptComparison);
		Subject.All.Dump("using a comparison on S");

		// adapt comparer from operators
		var adaptFromOperators = Cmp<Subject>.FromOperators() as OperatorComparer<Subject>;
		Subject.All.Sort(adaptFromOperators);
		Subject.All.Dump("using operators on D");
	}
}