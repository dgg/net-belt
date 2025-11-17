using System.Diagnostics.CodeAnalysis;

using Net.Belt.Patterns.Specification;
using Net.Belt.Tests.Patterns.Specification.Support;

using Iz = Net.Belt.Tests.Patterns.Specification.Support.Iz;

namespace Net.Belt.Tests.Patterns.Specification;

[TestFixture]
[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class PredicateSpecificationTester
{
	#region interface operations

	[Test]
	public void ISpecification_LengthBetween5And10()
	{
		// defined inline without new types
		ISpecification<string> subject = new PredicateSpecification<string>(s => s.Length is >= 5 and <= 10);

		Assert.That(subject, Iz.SatisfiedBy("123456"));
		Assert.That(subject, Is.Not.SatisfiedBy("1234").Or("1234567890123"));
	}

	[Test]
	public void Not_LengthBetween5And10()
	{
		ISpecification<string> lengthBetween5And10 =
			new PredicateSpecification<string>(s => s.Length is >= 5 and <= 10);
		ISpecification<string> subject = lengthBetween5And10.Not();

		Assert.That(subject, Is.Not.SatisfiedBy("123456"));
		Assert.That(subject, Iz.SatisfiedBy("1234").And("1234567890123"));
	}

	[Test]
	public void MoreThan5_And_LessThan10()
	{
		ISpecification<int> lessThan10 = PredicateSpecification.For<int>(i => i < 10);
		ISpecification<int> moreThan5 = new PredicateSpecification<int>(i => i > 5);
		ISpecification<int> subject = lessThan10.And(moreThan5);

		Assert.That(subject, Iz.SatisfiedBy(7));
		Assert.That(subject, Is.Not.SatisfiedBy(3).Or(13));
	}

	[Test]
	public void MoreThan10_Or_LessThan5()
	{
		ISpecification<int> moreThan10 = PredicateSpecification.For<int>(i => i > 10);
		ISpecification<int> lessThan5 = new PredicateSpecification<int>(i => i < 5);
		ISpecification<int> subject = lessThan5.Or(moreThan10);

		Assert.That(subject, Is.Not.SatisfiedBy(7));
		Assert.That(subject, Iz.SatisfiedBy(3).And(13));
	}

	#endregion

	#region Operators

	[Test]
	public void NotOp_LengthBetween5And10()
	{
		var lengthBetween5And10 = new PredicateSpecification<string>(s => s.Length >= 5 && s.Length <= 10);
		PredicateSpecification<string> subject = !lengthBetween5And10;

		Assert.That(subject, Is.Not.SatisfiedBy("123456"));
		Assert.That(subject, Iz.SatisfiedBy("1234").And("1234567890123"));
	}

	[Test]
	public void MoreThan5_AndOp_LessThan10()
	{
		PredicateSpecification<int> lessThan10 = PredicateSpecification.For<int>(i => i < 10);
		var moreThan5 = new PredicateSpecification<int>(i => i > 5);
		PredicateSpecification<int> subject = lessThan10 && moreThan5;

		Assert.That(subject, Iz.SatisfiedBy(7));
		Assert.That(subject, Is.Not.SatisfiedBy(3).Or(13));
	}

	[Test]
	public void MoreThan10_OrOp_LessThan5()
	{
		PredicateSpecification<int> moreThan10 = PredicateSpecification.For<int>(i => i > 10);
		var lessThan5 = new PredicateSpecification<int>(i => i < 5);
		PredicateSpecification<int> subject = lessThan5 || moreThan10;

		Assert.That(subject, Is.Not.SatisfiedBy(7));
		Assert.That(subject, Iz.SatisfiedBy(3).And(13));
	}

	[Test]
	public void Explicit_To_Predicate()
	{
		var lessThan5 = new PredicateSpecification<int>(i => i < 5);
		var l = new List<int>(new[] { 2, 4, 6, 8, 10 });
		Predicate<int> p = lessThan5.Predicate;
		Assert.That(l.FindAll(p), Has.Count.EqualTo(2));
	}

	[Test]
	public void Explicit_To_Fucntion()
	{
		var lessThan5 = new PredicateSpecification<int>(i => i < 5);
		var a = new[] { 2, 4, 6, 8, 10 };
		Func<int, bool> p = lessThan5.Function;
		Assert.That(a.Where(p), Has.Exactly(2).Items);
	}

	[Test]
	public void Implicit_To_Predicate()
	{
		var lessThan5 = new PredicateSpecification<int>(i => i < 5);
		var l = new List<int>([2, 4, 6, 8, 10]);
		Predicate<int> p = lessThan5;
		Assert.That(l.FindAll(p), Has.Count.EqualTo(2));
	}

	[Test]
	public void Implicit_To_Function()
	{
		var lessThan5 = new PredicateSpecification<int>(i => i < 5);
		var a = new[] { 2, 4, 6, 8, 10 };
		Func<int, bool> p = lessThan5;
		Assert.That(a.Where(p), Has.Exactly(2).Items);
	}

	#endregion

	#region class encapsulation tests

	[Test]
	public void Encapsulated_LessThan10_ExpectedBehavior()
	{
		var subject = new LessThan10SpecSubject();

		Assert.That(subject, Iz.SatisfiedBy(5));
		Assert.That(subject, Is.Not.SatisfiedBy(11));
	}

	[Test]
	public void Encapsulated_NegatedMoreThan5_ExpectedBehavior()
	{
		var subject = new MoreThan5SpecSubject().Not();

		Assert.That(subject, Is.Not.SatisfiedBy(6));
		Assert.That(subject, Iz.SatisfiedBy(3));
	}

	[Test]
	public void Encapsulated_AndComposition_ExpectedBehavior()
	{
		var lessThan10 = new LessThan10SpecSubject();
		var moreThan5 = new MoreThan5SpecSubject();

		var subject = lessThan10.And(moreThan5);
		Assert.That(subject, Iz.SatisfiedBy(7));
		Assert.That(subject, Is.Not.SatisfiedBy(3).Or(13));

		subject = lessThan10.And(moreThan5);
		Assert.That(subject, Iz.SatisfiedBy(7));
		Assert.That(subject, Is.Not.SatisfiedBy(3).Or(13));
	}

	[Test]
	public void Encapsulated_OrComposition_ExpectedBehavior()
	{
		Specification<int> moreThan10 = new MoreThan10SpecSubject();
		Specification<int> lessThan5 = new LessThan5SpecSubject();

		ISpecification<int> subject = lessThan5.Or(moreThan10);
		Assert.That(subject, Is.Not.SatisfiedBy(7));
		Assert.That(subject, Iz.SatisfiedBy(3).And(13));

		subject = lessThan5.Or(moreThan10);
		Assert.That(subject, Is.Not.SatisfiedBy(7));
		Assert.That(subject, Iz.SatisfiedBy(3).And(13));
	}

	#endregion

	#region complex composition

	private readonly PredicateSpecification<ComplexType> _fooLengthOf2 = new(c => c.Foo.Length == 2);

	[Test]
	public void ComplexComposition_PredicateUsage_FoundMatchingElements()
	{
		var data = new List<ComplexType>(new ComplexContainer());
		Assert.That(data.Find(_fooLengthOf2), Has.Property(nameof(ComplexType.Bar)).EqualTo(2));

		Assert.That(data.FindIndex(!new BarEven()), Is.EqualTo(2));

		Specification<ComplexType> enabled = new ComplexTypeEnabled(), barEven = new BarEven();
		Predicate<ComplexType> enabledOrDisabledAndBarEven = c =>
			enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
		Assert.That(data.FindAll(enabledOrDisabledAndBarEven), Has.Count.EqualTo(6));
	}

	[Test]
	public void ComplexComposition_LinqUsage_FoundMatchingElements()
	{
		IEnumerable<ComplexType> data = new ComplexContainer();
		var q1 = from c in data where _fooLengthOf2.IsSatisfiedBy(c) select c.Bar;
		Assert.That(q1.First(), Is.EqualTo(2));

		var q2 = data.Where(!new BarEven()).Select(c => c.Foo);
		Assert.That(q2.First(), Is.EqualTo("12"));

		Specification<ComplexType> enabled = new ComplexTypeEnabled(), barEven = new BarEven();
		Func<ComplexType, bool> enabledOrDisabledAndBarEven = c =>
			enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
		var q3 = from c in data where enabledOrDisabledAndBarEven(c) select c;
		Assert.That(q3, Has.Exactly(6).Items);
	}

	#endregion
}