using Net.Belt.Tests.ValueObjects.Support;
using Net.Belt.ValueObjects;

using NUnit.Framework.Constraints;

using Testing.Commons.Time;

using Iz = Net.Belt.Tests.ValueObjects.Support.Iz;

namespace Net.Belt.Tests.ValueObjects;

[TestFixture]
public class AgeTester
{
	#region construction
	
	#region ctor
	
	[Test]
	public void Constructor_TerminusEarlierThanAdvent_ThrowsArgumentOutOfRangeException()
	{
		var advent = new DateOnly(2023, 1, 1);
		var terminus = new DateOnly(2022, 12, 31);

		Assert.That(() => new Age(advent, terminus), Throws.InstanceOf<ArgumentOutOfRangeException>());
	}

	[Test]
	public void Ctor_OneDay_SetsProps()
	{
		DateOnly advent = 10.March(1977);
		DateOnly terminus = 11.March(1977);

		var subject = new Age(advent, terminus);

		Assert.That(subject, Iz.Bounded(advent, terminus));
		Assert.That(subject, Haz.Components(years: 0, months: 0, days: 1));
		Assert.That(subject, Haz.Span(1.Day()));
	}

	[Test]
	public void Ctor_OneWeek_SetsProps()
	{
		DateOnly advent = 10.March(1977);
		DateOnly terminus = 17.March(1977);

		var subject = new Age(advent, terminus);

		Assert.That(subject, Iz.Bounded(advent, terminus));
		Assert.That(subject, Haz.Components(years: 0, months: 0, days: 7));
		Assert.That(subject, Haz.Span(7.Days()));
	}

	[Test]
	public void Ctor_TwoMonthsThreeDays_SetsProps()
	{
		DateTime start = 10.March(1977),
			end = start.AddMonths(2).AddDays(3);

		DateOnly advent = DateOnly.FromDateTime(start),
			terminus = DateOnly.FromDateTime(end);

		var subject = new Age(advent, terminus);

		Assert.That(subject, Iz.Bounded(advent, terminus));
		Assert.That(subject, Haz.Components(years: 0, months: 2, days: 3));
		Assert.That(subject, Haz.Span(64.Days()));
	}

	[Test]
	public void Ctor_365Days_OneYearOld()
	{
		DateOnly advent = 1.December(2002), terminus = 1.December(2003);

		var subject = new Age(advent, terminus);

		Assert.That(subject, Iz.Bounded(advent, terminus));
		Assert.That(subject, Haz.Components(years: 1, months: 0, days: 0));
		Assert.That(subject, Haz.Span(365.Days()));
	}

	[Test]
	public void Ctor_366Days_OneYearOld()
	{
		DateOnly advent = 1.December(2003), terminus = 1.December(2004);

		var subject = new Age(advent, terminus);

		Assert.That(subject, Iz.Bounded(advent, terminus));
		Assert.That(subject, Haz.Components(years: 1, months: 0, days: 0));
		Assert.That(subject, Haz.Span(366.Days()));
	}

	[Test]
	public void Ctor_EdgeCaseWithNonLeapFeb_PropsSet()
	{
		DateOnly advent = 31.January(2023);
		DateOnly terminus = 1.March(2023);

		var subject = new Age(advent, terminus);

		Assert.That(subject, Haz.Components(years: 0, months: 1, days: 1),
			"Feb 1 to March 1 is 1 month, Jan 31 to Feb 28 is 28 days, so 1 day from Feb 28 to March 1");
	}
	
	#endregion
	
	#endregion

	[Test]
	public void Empty_SetMinProps()
	{
		Age subject = Age.Empty;
		Assert.That(subject, Iz.Bounded(DateOnly.MinValue, DateOnly.MinValue));
		Assert.That(subject, Haz.Components(years: 0, months: 0, days: 0));
		Assert.That(subject, Haz.Span(TimeSpan.Zero));
		Assert.That(subject.IsEmpty, Is.True);
	}

	#region equality

	[Test]
	public void Equality_SameAdventAndTerminus_Equals()
	{
		DateOnly advent = 11.March(1977),
			sameAdvent = new(1977, 3, 11);
		DateOnly terminus = 1.December(2025),
			sameTerminus = new(2025, 12, 1);

		Age subject = new(advent, terminus),
			sameBounds = new(sameAdvent, sameTerminus);

		Assert.That(subject.Equals(sameBounds), Is.True);
		Assert.That(sameBounds == subject, Is.True);
		Assert.That(subject != sameBounds, Is.False);
	}

	[Test]
	public void Equality_DifferentAdventAndTerminus_NotEquals()
	{
		DateOnly advent = 11.March(1977),
			differentAdvent = new(1977, 3, 2);
		DateOnly terminus = 1.December(2025),
			differentTerminus = new(2024, 12, 1);

		Age subject = new(advent, terminus),
			sameAdvent = new(advent, differentTerminus),
			sameTerminus = new(differentAdvent, terminus),
			differentBounds = new(differentAdvent, differentTerminus);

		Assert.That(subject.Equals(sameAdvent), Is.False);
		Assert.That(sameTerminus.Equals(subject), Is.False);
		Assert.That(subject.Equals(differentBounds), Is.False);

		Assert.That(sameAdvent == subject, Is.False);
		Assert.That(subject == sameTerminus, Is.False);
		Assert.That(differentBounds == subject, Is.False);

		Assert.That(subject != sameAdvent, Is.True);
		Assert.That(sameTerminus != subject, Is.True);
		Assert.That(subject != differentBounds, Is.True);
	}

	[Test]
	public void Equality_SameSpanDifferentBounds_NotEquals()
	{
		var oneYearOld = new Age(1.December(2024), 1.December(2025));
		var anotherYearOld = new Age(1.September(2024), 1.September(2025));

		Assert.That(oneYearOld.Equals(anotherYearOld), Is.False);
		Assert.That(anotherYearOld == oneYearOld, Is.False);
		Assert.That(oneYearOld != anotherYearOld, Is.True);
	}

	#endregion

	#region comparisons

	private static readonly Age _oneYearOld = new(1.December(2024), 1.December(2025));
	private static readonly Age _365daysOld = new(1.December(2001), 1.December(2002));
	private static readonly Age _366daysOld = new(1.December(2003), 1.December(2004));
	private static readonly Age _twoYearsOld = new(1.December(2023), 1.December(2025));

	private static readonly TestCaseData<Age, Age, Constraint>[] _compareToAge =
	[
		testCase(_oneYearOld, _oneYearOld, Is.Zero),
		testCase(_oneYearOld, _365daysOld, Is.Zero),
		testCase(_oneYearOld, _twoYearsOld, Is.LessThan(0)),
		testCase(_twoYearsOld, _oneYearOld, Is.GreaterThan(0)),
		testCase(_oneYearOld, _366daysOld, Is.LessThan(0)),
		testCase(_366daysOld, _oneYearOld, Is.GreaterThan(0)),
	];

	private static TestCaseData<Age, Age, Constraint> testCase(Age left, Age right, Constraint comparisonConstraint) =>
		new(left, right, comparisonConstraint);

	[Test, TestCaseSource(nameof(_compareToAge))]
	public void CompareTo_Age_Spec(Age left, Age right, Constraint comparisonConstraint) =>
		Assert.That(left.CompareTo(right), comparisonConstraint);

	[Test, TestCaseSource(nameof(_compareToAge))]
	public void CompareTo_BoxedAge_Spec(Age left, object? right, Constraint comparisonConstraint) =>
		Assert.That(left.CompareTo(right), comparisonConstraint);

	[Test]
	public void CompareTo_BoxedNull_GreaterThanZero() =>
		Assert.That(() => Age.Empty.CompareTo(null), Is.GreaterThan(0));
	
	[Test]
	public void CompareTo_BoxedUnsupported_Exception() =>
		Assert.That(() => Age.Empty.CompareTo(1), Throws.ArgumentException
			.With.Message.Contains(nameof(Age))
			.With.Message.Contains(nameof(TimeSpan)));

	private static readonly TestCaseData<Age, TimeSpan, Constraint>[] _compareToSpan =
	[
		testCase(_oneYearOld, 365.Days(), Is.Zero),
		testCase(_oneYearOld, (365 * 2).Days(), Is.LessThan(0)),
		testCase(_twoYearsOld, 365.Days(), Is.GreaterThan(0)),
		testCase(_oneYearOld, 366.Days(), Is.LessThan(0)),
		testCase(_366daysOld, 365.Days(), Is.GreaterThan(0)),
	];

	private static TestCaseData<Age, TimeSpan, Constraint> testCase(Age left, TimeSpan right,
		Constraint comparisonConstraint) =>
		new(left, right, comparisonConstraint);

	[Test, TestCaseSource(nameof(_compareToSpan))]
	public void CompareTo_Age_Spec(Age left, TimeSpan right, Constraint comparisonConstraint) =>
		Assert.That(left.CompareTo(right), comparisonConstraint);


	[Test, TestCaseSource(nameof(_compareToSpan))]
	public void CompareTo_BoxedSpan_Spec(Age left, object? right, Constraint comparisonConstraint) =>
		Assert.That(left.CompareTo(right), comparisonConstraint);

	private static readonly TestCaseData<Age, Age, Constraint>[] _greaterThan =
	[
		testCase(_oneYearOld, _oneYearOld, Is.False),
		testCase(_oneYearOld, _365daysOld, Is.False),
		testCase(_oneYearOld, _twoYearsOld, Is.False),
		testCase(_twoYearsOld, _oneYearOld, Is.True),
		testCase(_oneYearOld, _366daysOld, Is.False),
		testCase(_366daysOld, _oneYearOld, Is.True),
	];

	[Test, TestCaseSource(nameof(_greaterThan))]
	public void GreaterThan_Age_Spec(Age left, Age right, Constraint comparisonConstraint) =>
		Assert.That(left > right, comparisonConstraint);

	private static readonly TestCaseData<Age, Age, Constraint>[] _greaterThanOrEqual =
	[
		testCase(_oneYearOld, _oneYearOld, Is.True),
		testCase(_oneYearOld, _365daysOld, Is.True),
		testCase(_oneYearOld, _twoYearsOld, Is.False),
		testCase(_twoYearsOld, _oneYearOld, Is.True),
		testCase(_oneYearOld, _366daysOld, Is.False),
		testCase(_366daysOld, _oneYearOld, Is.True),
	];

	[Test, TestCaseSource(nameof(_greaterThanOrEqual))]
	public void GreaterThanOrEqual_Age_Spec(Age left, Age right, Constraint comparisonConstraint) =>
		Assert.That(left >= right, comparisonConstraint);

	private static readonly TestCaseData<Age, Age, Constraint>[] _lessThan =
	[
		testCase(_oneYearOld, _oneYearOld, Is.False),
		testCase(_oneYearOld, _365daysOld, Is.False),
		testCase(_oneYearOld, _twoYearsOld, Is.True),
		testCase(_twoYearsOld, _oneYearOld, Is.False),
		testCase(_oneYearOld, _366daysOld, Is.True),
		testCase(_366daysOld, _oneYearOld, Is.False),
	];

	[Test, TestCaseSource(nameof(_lessThan))]
	public void LessThan_Age_Spec(Age left, Age right, Constraint comparisonConstraint) =>
		Assert.That(left < right, comparisonConstraint);

	private static readonly TestCaseData<Age, Age, Constraint>[] _lessThanOrEqual =
	[
		testCase(_oneYearOld, _oneYearOld, Is.True),
		testCase(_oneYearOld, _365daysOld, Is.True),
		testCase(_oneYearOld, _twoYearsOld, Is.True),
		testCase(_twoYearsOld, _oneYearOld, Is.False),
		testCase(_oneYearOld, _366daysOld, Is.True),
		testCase(_366daysOld, _oneYearOld, Is.False),
	];

	[Test, TestCaseSource(nameof(_lessThanOrEqual))]
	public void LessThanOrEqual_Age_Spec(Age left, Age right, Constraint comparisonConstraint) =>
		Assert.That(left <= right, comparisonConstraint);

	#endregion

	[Test]
	public void ToString_RecordLikeCultureIndependent_BoundsAndComponents()
	{
		var oneDay = new Age(30.November(2025), 1.December(2025));
		Assert.That(oneDay.ToString(),
			Is.EqualTo("Age { Advent = 2025-11-30, Terminus = 2025-12-01, Years = 0, Months = 0, Days = 1 }"));
		
		var oneYearAndOneMonth = new Age(1.November(2024), 1.December(2025));
		Assert.That(oneYearAndOneMonth.ToString(),
			Is.EqualTo("Age { Advent = 2024-11-01, Terminus = 2025-12-01, Years = 1, Months = 1, Days = 0 }"));
		
		var twentyFiveYearsAndOneDay = new Age(30.November(2000), 1.December(2025));
		Assert.That(twentyFiveYearsAndOneDay.ToString(),
			Is.EqualTo("Age { Advent = 2000-11-30, Terminus = 2025-12-01, Years = 25, Months = 0, Days = 1 }"));
	}
}