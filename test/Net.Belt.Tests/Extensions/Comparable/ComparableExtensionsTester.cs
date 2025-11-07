using System.Collections;

using Net.Belt.Extensions.Comparable;
using Net.Belt.Tests.Extensions.Comparable.Support;

namespace Net.Belt.Tests.Extensions.Comparable;

[TestFixture]
public class ComparableExtensionsTester
{
	[Test]
	public void IsEqualTo()
	{
		Assert.That(6.IsEqualTo(5), Is.False);
		Assert.That(6.IsEqualTo(7), Is.False);
		Assert.That(6.IsEqualTo(6), Is.True);

		// IComparable
		var subject = new ComparableSubject(6);
		Assert.That(subject.IsEqualTo(5), Is.False);
		Assert.That(subject.IsEqualTo(7), Is.False);
		Assert.That(subject.IsEqualTo(6), Is.True);

		// IComparable<T>
		var genericSubject = new GenericComparableSubject<int>(6);
		Assert.That(genericSubject.IsEqualTo(new GenericComparableSubject<int>(5)), Is.False);
		Assert.That(genericSubject.IsEqualTo(new GenericComparableSubject<int>(6)), Is.True);
		Assert.That(genericSubject.IsEqualTo(new GenericComparableSubject<int>(7)), Is.False);
	}

	[Test]
	public void IsDifferentFrom()
	{
		Assert.That(6.IsDifferentFrom(5), Is.True);
		Assert.That(6.IsDifferentFrom(7), Is.True);
		Assert.That(6.IsDifferentFrom(6), Is.False);

		var subject = new ComparableSubject(6);
		Assert.That(subject.IsDifferentFrom(5), Is.True);
		Assert.That(subject.IsDifferentFrom(7), Is.True);
		Assert.That(subject.IsDifferentFrom(6), Is.False);

		var genericSubject = new GenericComparableSubject<int>(6);
		Assert.That(genericSubject.IsDifferentFrom(new GenericComparableSubject<int>(5)), Is.True);
		Assert.That(genericSubject.IsDifferentFrom(new GenericComparableSubject<int>(6)), Is.False);
		Assert.That(genericSubject.IsDifferentFrom(new GenericComparableSubject<int>(7)), Is.True);
	}

	[Test]
	public void IsAtMost()
	{
		Assert.That(6.IsAtMost(5), Is.False);
		Assert.That(6.IsAtMost(7), Is.True);
		Assert.That(6.IsAtMost(6), Is.True);

		var subject = new ComparableSubject(6);
		Assert.That(subject.IsAtMost(5), Is.False);
		Assert.That(subject.IsAtMost(7), Is.True);
		Assert.That(subject.IsAtMost(6), Is.True);

		var genericSubject = new GenericComparableSubject<int>(6);
		Assert.That(genericSubject.IsAtMost(new GenericComparableSubject<int>(5)), Is.False);
		Assert.That(genericSubject.IsAtMost(new GenericComparableSubject<int>(6)), Is.True);
		Assert.That(genericSubject.IsAtMost(new GenericComparableSubject<int>(7)), Is.True);
	}

	[Test]
	public void IsAtLeast()
	{
		Assert.That(6.IsAtLeast(3), Is.True);
		Assert.That(6.IsAtLeast(8), Is.False);
		Assert.That(6.IsAtLeast(6), Is.True);

		var subject = new ComparableSubject(6);
		Assert.That(subject.IsAtLeast(3), Is.True);
		Assert.That(subject.IsAtLeast(8), Is.False);
		Assert.That(subject.IsAtLeast(6), Is.True);

		var genericSubject = new GenericComparableSubject<int>(6);
		Assert.That(genericSubject.IsAtLeast(new GenericComparableSubject<int>(3)), Is.True);
		Assert.That(genericSubject.IsAtLeast(new GenericComparableSubject<int>(8)), Is.False);
		Assert.That(genericSubject.IsAtLeast(new GenericComparableSubject<int>(6)), Is.True);
	}

	[Test]
	public void IsLessThan()
	{
		Assert.That(6.IsLessThan(3), Is.False);
		Assert.That(6.IsLessThan(8), Is.True);
		Assert.That(6.IsLessThan(6), Is.False);

		var subject = new ComparableSubject(6);
		Assert.That(subject.IsLessThan(3), Is.False);
		Assert.That(subject.IsLessThan(8), Is.True);
		Assert.That(subject.IsLessThan(6), Is.False);

		var genericSubject = new GenericComparableSubject<int>(6);
		Assert.That(genericSubject.IsLessThan(new GenericComparableSubject<int>(3)), Is.False);
		Assert.That(genericSubject.IsLessThan(new GenericComparableSubject<int>(8)), Is.True);
		Assert.That(genericSubject.IsLessThan(new GenericComparableSubject<int>(6)), Is.False);
	}

	[Test]
	public void IsMoreThan()
	{
		Assert.That(6.IsMoreThan(3), Is.True);
		Assert.That(6.IsMoreThan(8), Is.False);
		Assert.That(6.IsMoreThan(6), Is.False);

		var subject = new ComparableSubject(6);
		Assert.That(subject.IsMoreThan(3), Is.True);
		Assert.That(subject.IsMoreThan(8), Is.False);
		Assert.That(subject.IsMoreThan(6), Is.False);

		var genericSubject = new GenericComparableSubject<int>(6);
		Assert.That(genericSubject.IsMoreThan(new GenericComparableSubject<int>(3)), Is.True);
		Assert.That(genericSubject.IsMoreThan(new GenericComparableSubject<int>(8)), Is.False);
		Assert.That(genericSubject.IsMoreThan(new GenericComparableSubject<int>(6)), Is.False);
	}

	[Test]
	public void Extensions_OnStrings()
	{
		string a = "a", b = "b";

		Assert.That(a.IsAtLeast(a), Is.True);
		Assert.That(a.IsAtMost(a), Is.True);
		Assert.That(a.IsLessThan(a), Is.False);
		Assert.That(a.IsMoreThan(a), Is.False);

		Assert.That(a.IsAtLeast(b), Is.False);
		Assert.That(a.IsAtMost(b), Is.True);
		Assert.That(a.IsLessThan(b), Is.True);
		Assert.That(a.IsMoreThan(b), Is.False);
	}

	[Test]
	public void Reverse_InvertResults()
	{
		var five = new GenericComparableSubject<int>(5);
		var six = new GenericComparableSubject<int>(6);
		IComparer<GenericComparableSubject<int>> comparer = Comparer<GenericComparableSubject<int>>.Default;

		// sanity check
		Assert.That(six.IsEqualTo(five), Is.False);
		Assert.That(six.IsEqualTo(six), Is.True);
		Assert.That(comparer.Compare(six, five), Is.Not.EqualTo(0));
		Assert.That(comparer.Compare(six, six), Is.EqualTo(0));
		
		Assert.That(six.IsMoreThan(five), Is.True);
		Assert.That(six.IsLessThan(six), Is.False);
		Assert.That(comparer.Compare(six, five), Is.GreaterThan(0));
		Assert.That(comparer.Compare(five, six), Is.LessThan(0));
		
		// reverse
		IComparer<GenericComparableSubject<int>> subject = comparer.Reverse();
		Assert.That(subject.Compare(six, five), Is.Not.EqualTo(0), "no change in equality");
		Assert.That(subject.Compare(six, six), Is.EqualTo(0), "no change in equality");
		
		Assert.That(subject.Compare(six, five), Is.LessThan(0), "change in order");
		Assert.That(subject.Compare(five, six), Is.GreaterThan(0), "change in order");
	}
}