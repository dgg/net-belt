using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Dumpify;

using Net.Belt.Patterns.Specification;

namespace Net.Belt.Tests.Patterns.Specification;

[TestFixture, Category("showcase"), Explicit("noisy")]
public class SpecificationShowcase
{
	class OddSpec : Specification<int>
	{
		public override bool IsSatisfiedBy(int item) => (item % 2) != 0;
	}

	class PlentySpec : Specification<int>
	{
		public override bool IsSatisfiedBy(int item) => item > 5;
	}

	[Test]
	public void Classic()
	{
		ISpecification<int> oddness = new OddSpec();
		oddness.IsSatisfiedBy(11)
			.Dump("eleven is pretty odd");
		oddness.IsSatisfiedBy(42)
			.Dump("42? not so much");

		// negate
		ISpecification<int> evenness = oddness.Not();
		evenness.IsSatisfiedBy(42)
			.Dump("but 42 makes a pretty good not-odd number");

		// compose
		ISpecification<int> plentyOfOddness = new PlentySpec()
			.And(oddness);
		plentyOfOddness.IsSatisfiedBy(5)
			.Dump("five is not that plenty");
		plentyOfOddness.IsSatisfiedBy(7)
			.Dump("seven, on the other hand...");

		var easyToPlease = oddness.Or(evenness);
		easyToPlease.IsSatisfiedBy(1)
			.Dump("odds will do");
		easyToPlease.IsSatisfiedBy(2)
			.Dump("and so will evens");

		// complex composition flows
		var crazy = oddness.Or(evenness.Not()).And(plentyOfOddness);
		crazy.IsSatisfiedBy(7)
			.Dump("surely satisfied");
		crazy.IsSatisfiedBy(8)
			.Dump("but of course not!");

		// clumsy to use with LINQ
		Enumerable.Range(0, 10)
			.Where(oddness.IsSatisfiedBy)
			.Dump("some odd numbers");
	}

	// naming helps
	static class Numbers
	{
		public static readonly Predicate<int> Plenty = i => i > 5;
	}

	[Test]
	[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
	public void Predicates()
	{
		Predicate<int> oddness = i => (i % 2) != 0;
		oddness(11)
			.Dump("eleven is pretty odd");
		oddness(42)
			.Dump("42? not so much");

		// predicate composition does not flow but can be operated on
		Func<int, bool> evenness = i => !oddness(i);
		evenness(42)
			.Dump("but 42 makes a pretty good not-odd number");

		Predicate<int> easyToPlease = i => oddness(i) || evenness(i);
		easyToPlease(1)
			.Dump("odds will do");
		easyToPlease(2)
			.Dump("and so will evens");

		// with a little help from my names
		Predicate<int> plentyOfOddness = i => Numbers.Plenty(i) && oddness(i);
		plentyOfOddness(5)
			.Dump("five is not that plenty");
		plentyOfOddness(7)
			.Dump("seven, on the other hand...");

		// complex composition gets crazier
		Func<int, bool> crazy = i => (oddness(i) || !evenness(i)) && plentyOfOddness(i);
		crazy(7)
			.Dump("surely satisfied");
		crazy(8)
			.Dump("but of course not!");
	}

	// not inline if predicate was more complicated
	class OddPredicate() : PredicateSpecification<int>(predicate)
	{
		private static bool predicate(int item) => (item % 2) != 0;
	}

	// inline for simple predicates
	class PlentyPredicate() : PredicateSpecification<int>(i => i > 5);

	[Test]
	public void EncapsulatedPredicates()
	{
		var isOdd = new OddPredicate();

		// predicate specs ARE specs
		isOdd.IsSatisfiedBy(11)
			.Dump("eleven is pretty odd");
		isOdd.Not().IsSatisfiedBy(42)
			.Dump("but 42 makes a pretty good not-odd number");
		new PlentyPredicate().And(isOdd).IsSatisfiedBy(5)
			.Dump("five is not that plenty");

		// better LINQ usage...
		Enumerable.Range(0, 10)
			.Where(isOdd)
			.Dump("some odd numbers");

		// ...via implicit magic
		Predicate<int> predicate = isOdd.Predicate;
		predicate = isOdd;

		Func<int, bool> func = isOdd.Function;
		func = isOdd;

		var huge = PredicateSpecification.For<int>(i => i > 10)
			.Dump("in place creation");

		(huge && !isOdd).IsSatisfiedBy(16)
			.Dump("HUGE and even via operators");
	}
	
	// a little more clumsy to not inline
	class OddExpression() : ExpressionSpecification<int>(expression)
	{
		private static Expression<Func<int, bool>> expression = i => (i % 2) != 0;
	}

	// inline for simple expressions
	class PlentyExpression() : ExpressionSpecification<int>(i => i > 5);

	class Entity 
	{
		public long Id { get; set; }
	}

	[Test]
	public void EncapsulatedExpressions()
	{
		var isOdd = new OddExpression();

		// expression specs ARE specs
		isOdd.IsSatisfiedBy(11)
			.Dump("eleven is pretty odd");
		isOdd.Not().IsSatisfiedBy(42)
			.Dump("but 42 makes a pretty good even number");
		new PlentyExpression().And(isOdd).IsSatisfiedBy(5)
			.Dump("five is not that plenty");

		// equally LINQ friendly
		Enumerable.Range(0, 10)
			.Where(isOdd)
			.Dump("some odd numbers");

		var huge = ExpressionSpecification.For<int>(i => i > 10)
			.Dump("in place", 2);
		(huge && !isOdd).IsSatisfiedBy(16)
			.Dump("HUGE and even with operators");

		// beyond Linq-to-objects
		var skipOne = ExpressionSpecification.For<Entity>(e => e.Id >= 2);
	
		Enumerable.Range(1, 4)
			.Select(n => new Entity { Id = n })
			.Where(skipOne)
			.Dump();
	
		// usable to be translated by other LINQ providers
		/*new MongoClient()
			.GetDatabase("test")
			.GetCollection<Entity>("test")
			.AsQueryable()
			.Where(skipOne)
			.Dump();*/
	}
}