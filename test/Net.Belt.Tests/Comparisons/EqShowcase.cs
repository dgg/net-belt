using Dumpify;

using Net.Belt.Comparisons;

namespace Net.Belt.Tests.Comparisons;

[TestFixture, Category("showcase")]
public class EqShowcase
{
	internal class Subject
	{
		public required string S { get; set; }
		public int I { get; set; }
		public decimal D { get; set; }

		public static Subject a = new() { S = "A", I = 4, D = 7.61m };
		public static Subject b = new() { S = "b", I = 1, D = 3.00m };
		public static Subject c = new() { S = "C", I = 3, D = 7.60m };
		public static Subject d = new() { S = "D", I = 2, D = 3.00m };
		public static Subject e = new() { S = "E", I = 5, D = 6.40m };

		public static List<Subject> All = [e, d, b, a, c];
	}
	
	internal class MyEqualizer : ChainableEqualizer<Subject>
	{
		// implement equality based on members of Subject
		protected override bool DoEquals(Subject x, Subject y) => false;

		// generate a hash code on members of Subject
		protected override int DoGetHashCode(Subject x) => 0;
	}

	[Test]
	public void ChainableEqualizers()
	{
		// instead of declaring classes, we can the Eq factory
		
		// create an equalizer by property
		ChainableEqualizer<Subject> byProperties = Eq<Subject>.By(s => s.D);
	
		byProperties.Equals(Subject.b, Subject.d)
			.Dump("same D");
	
		byProperties
			.Then(s => s.I)
			.Equals(Subject.b, Subject.d)
			.Dump("different I");

		// use a full-blown delegate for customization
		var byDelegate = Eq<Subject>.By(
			(x, y) => StringComparer.Ordinal.Equals(x.S, y.S),
			s => StringComparer.Ordinal.GetHashCode(s.S));

		// Hasher
		Hasher.Zero(1)
			.Dump("always zero");
		Hasher.Default(new Exception())
			.Dump("whatever .GetHashCode() returns");
	}
}