using System.Diagnostics.CodeAnalysis;

using Net.Belt.Patterns.Specification;
using Net.Belt.Tests.Patterns.Specification.Support;

using Iz = Net.Belt.Tests.Patterns.Specification.Support.Iz;

namespace Net.Belt.Tests.Patterns.Specification;

[TestFixture]
[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class SpecificationTester
{
	[Test]
	public void IsSatisfiedBy_TrueOrFalse()
	{
		var subject = new LessThan10();
		
		Assert.That(subject.IsSatisfiedBy(5), Is.True);
		Assert.That(subject.IsSatisfiedBy(11), Is.False);
		
		// use support constraints
		Assert.That(subject, Iz.SatisfiedBy(5));
		Assert.That(subject, Is.Not.SatisfiedBy(11));
	}
	
	#region interface operations

	[Test]
	public void ISpecification_LengthBetween5And10()
	{
		ISpecification<string> subject = new LengthBetween5And10();

		Assert.That(subject, Iz.SatisfiedBy("123456"));
		Assert.That(subject, Is.Not.SatisfiedBy("1234").Or("1234567890123"));
	}
	
	[Test]
	public void Not_LengthBetween5And10()
	{
		ISpecification<string> lengthBetween5And10 = new LengthBetween5And10();
		ISpecification<string> subject = lengthBetween5And10.Not();

		Assert.That(subject, Is.Not.SatisfiedBy("123456"));
		Assert.That(subject, Iz.SatisfiedBy("1234").And("1234567890123"));
	}

	[Test]
	public void MoreThan5_And_LessThan10()
	{
		ISpecification<int> lessThan10 = new LessThan10();
		ISpecification<int> moreThan5 = new MoreThan5();
		ISpecification<int> subject = lessThan10.And(moreThan5);

		Assert.That(subject, Iz.SatisfiedBy(7));
		Assert.That(subject, Is.Not.SatisfiedBy(3).Or(13));
	}

	[Test]
	public void MoreThan10_Or_LessThan5()
	{
		ISpecification<int> moreThan10 = new MoreThan10();
		ISpecification<int> lessThan5 = new LessThan5();
		ISpecification<int> subject = lessThan5.Or(moreThan10);

		Assert.That(subject, Is.Not.SatisfiedBy(7));
		Assert.That(subject, Iz.SatisfiedBy(3).And(13));
	}
	
	#endregion
	
	#region complex composition
	
	[Test]
	public void ComplexComposition_PredicateUsage_FoundMatchingElements()
	{
		var data = new List<ComplexType>(new ComplexContainer());
		Assert.That(data.Find(new Foo_LengthOf2().IsSatisfiedBy), Has.Property(nameof(ComplexType.Bar)).EqualTo(2));

		Assert.That(data.FindIndex(new Bar_Even().Not().IsSatisfiedBy), Is.EqualTo(2));

		Specification<ComplexType> enabled = new ComplexType_Enabled(), barEven = new Bar_Even();
		Predicate<ComplexType> enabledOrDisabledAndBarEven = c => enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
		Assert.That(data.FindAll(enabledOrDisabledAndBarEven), Has.Count.EqualTo(6));
	}

	[Test]
	public void ComplexComposition_LinqUsage_FoundMatchingElements()
	{
		IEnumerable<ComplexType> data = new ComplexContainer();
		var fooLengthOf2 = new Foo_LengthOf2();
		var q1 = from c in data where fooLengthOf2.IsSatisfiedBy(c) select c.Bar;
		Assert.That(q1.First(), Is.EqualTo(2));

		Func<ComplexType, bool> notEven = c => new Bar_Even().Not().IsSatisfiedBy(c);
		var q2 = data.Where(notEven).Select(c => c.Foo);
		Assert.That(q2.First(), Is.EqualTo("12"));

		Specification<ComplexType> enabled = new ComplexType_Enabled(), barEven = new Bar_Even();
		Func<ComplexType, bool> enabledOrDisabledAndBarEven = c => enabled.IsSatisfiedBy(c) || (!enabled.IsSatisfiedBy(c) && barEven.IsSatisfiedBy(c));
		var q3 = from c in data where enabledOrDisabledAndBarEven(c) select c;
		Assert.That(q3, Has.Exactly(6).Items);
	}
	
	#endregion
}