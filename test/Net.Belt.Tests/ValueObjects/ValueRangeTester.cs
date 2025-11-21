using System.Globalization;

using Net.Belt.Tests.ValueObjects.Support;
using Net.Belt.ValueObjects;

using Testing.Commons.Time;

namespace Net.Belt.Tests.ValueObjects;

[TestFixture]
public class ValueRangeTester
{
	#region construction

	[Test]
	public void Ctor_SetsProperties()
	{
		var subject = new ValueRange<char>('a', 'z');
		Assert.That(subject.Lower, Is.EqualTo('a'));
		Assert.That(subject.Upper, Is.EqualTo('z'));
	}

	[Test]
	public void Ctor_BoundsClosedByDefault()
	{
		var subject = new ValueRange<char>('a', 'z');
		Assert.That(subject.LowerBound.IsClosed, Is.True);
		Assert.That(subject.UpperBound.IsClosed, Is.True);
	}

	[Test, SetCulture("es")]
	public void Ctor_PoorlyConstructed_Exception()
	{
		Assert.That(() => new ValueRange<int>(5, 1), Throwz.BoundException(1, "1"));

		Assert.That(() => new ValueRange<int>(-1, -5), Throwz.BoundException(-5, "-5"));

		Assert.That(() => new ValueRange<TimeSpan>(3.Seconds(), 2.Seconds()),
			Throwz.BoundException(2.Seconds(), "00:00:02"));

		Assert.That(2.5.ToString(CultureInfo.CurrentCulture), Is.EqualTo("2,5"), "comma-separated decimals in Spain");
		Assert.That(() => new ValueRange<double>(3.2, 2.5),
			Throwz.BoundException(2.5, "2.5"));
	}

	[Test]
	public void Inference_NicerSyntax()
	{
		Assert.That(() => ValueRange.New(1, 5), Throws.Nothing);

		Assert.That(() => ValueRange.New(-5, -1), Throws.Nothing);

		Assert.That(() => ValueRange.New(2.Seconds(), 3.Seconds()), Throws.Nothing);

		Assert.That(() => ValueRange.New(31.October(1952), 11.March(1977)), Throws.Nothing);
	}

	[Test]
	public void Degenerate_SameBounds()
	{
		ValueRange<TimeSpan> degenerate = ValueRange.Degenerate(2.Seconds());

		Assert.That(degenerate.Lower, Is.EqualTo(2.Seconds()));
		Assert.That(degenerate.Upper, Is.EqualTo(2.Seconds()));
	}

	#endregion

	#region bound-checking

	#region CheckBounds

	[Test]
	public void CheckBounds_CorrectlyOrderedBounds_True()
	{
		Assert.That(ValueRange<char>.CheckBounds('a', 'z'), Is.True);
		Assert.That(ValueRange<int>.CheckBounds(-1, 1), Is.True);
		Assert.That(ValueRange<DateTime>.CheckBounds(11.March(1977), 9.September(2010)), Is.True);
		Assert.That(ValueRange<TimeSpan>.CheckBounds(3.Seconds(), 1.Hours()), Is.True);
	}

	[Test]
	public void CheckBounds_SameValueForBothBounds_True()
	{
		Assert.That(ValueRange<char>.CheckBounds('a', 'a'), Is.True);
		Assert.That(ValueRange<int>.CheckBounds(-1, -1), Is.True);
		Assert.That(ValueRange<DateTime>.CheckBounds(11.March(1977), 11.March(1977)), Is.True);
		Assert.That(ValueRange<TimeSpan>.CheckBounds(3.Seconds(), 3.Seconds()), Is.True);
	}

	[Test]
	public void CheckBounds_IncorrectlyOrderedBounds_False()
	{
		Assert.That(ValueRange<char>.CheckBounds('z', 'a'), Is.False);
		Assert.That(ValueRange<int>.CheckBounds(1, -1), Is.False);
		Assert.That(ValueRange<DateTime>.CheckBounds(9.September(2010), 11.March(1977)), Is.False);
		Assert.That(ValueRange<TimeSpan>.CheckBounds(1.Hours(), 3.Seconds()), Is.False);
	}

	[Test]
	public void Checkbounds_Inference_NicerSyntax()
	{
		Assert.That(ValueRange.CheckBounds('z', 'a'), Is.False);
		Assert.That(ValueRange.CheckBounds(11.March(1977), 11.March(1977)), Is.True);
	}

	#endregion

	#region AssertBounds

	[Test]
	public void AssertBounds_CorrectlyOrderedBounds_NoException()
	{
		Assert.That(() => ValueRange<char>.AssertBounds('a', 'z'), Throws.Nothing);
		Assert.That(() => ValueRange<int>.AssertBounds(-1, 1), Throws.Nothing);
		Assert.That(() => ValueRange<DateTime>.AssertBounds(11.March(1977), 9.September(2010)), Throws.Nothing);
		Assert.That(() => ValueRange<TimeSpan>.AssertBounds(3.Seconds(), 1.Hours()), Throws.Nothing);
	}

	[Test]
	public void AssertBound_SameValueForBothBounds_NoException()
	{
		Assert.That(() => ValueRange<char>.AssertBounds('a', 'a'), Throws.Nothing);
		Assert.That(() => ValueRange<int>.AssertBounds(-1, -1), Throws.Nothing);
		Assert.That(() => ValueRange<DateTime>.AssertBounds(11.March(1977), 11.March(1977)), Throws.Nothing);
		Assert.That(() => ValueRange<TimeSpan>.AssertBounds(3.Seconds(), 3.Seconds()), Throws.Nothing);
	}

	[Test, SetCulture("es")]
	public void AssertBounds_IncorrectlyOrderedBounds_Exception()
	{
		Assert.That(() => ValueRange<char>.AssertBounds('z', 'a'), Throwz.BoundException('a', "a"));
		Assert.That(() => ValueRange<int>.AssertBounds(1, -1), Throwz.BoundException(-1, "-1"));
		Assert.That(() => ValueRange<TimeSpan>.AssertBounds(1.Hours(), 3.Seconds()),
			Throwz.BoundException(3.Seconds(), "00:00:03"));
		Assert.That(() => ValueRange<DateTime>.AssertBounds(9.September(2010), 11.March(1977)),
			Throwz.BoundException(11.March(1977), "11/3/1977"));
	}

	[Test]
	public void AssertBounds_Inference_NicerSyntax()
	{
		Assert.That(() => ValueRange.AssertBounds('z', 'a'), Throwz.BoundException('a', "a"));
		Assert.That(() => ValueRange.AssertBounds(11.March(1977), 11.March(1977)), Throws.Nothing);
	}

	#endregion

	#region argument assertion

	[Test]
	public void AssertArgument_Closed_NotContained_Exception()
	{
		var subject = ValueRange.Closed(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 6),
			Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("[1..5]").And
				.With.Message.Contain("1 (inclusive)").And
				.With.Message.Contain("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
	}

	[Test]
	public void AssertArgument_Closed_Contained_NoException()
	{
		var subject = ValueRange.Closed(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
	}

	[Test]
	public void AssertArgument_Open_NotContained_Exception()
	{
		var subject = ValueRange.Open(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 6),
			Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("(1..5)").And
				.With.Message.Contain("1 (not inclusive)").And
				.With.Message.Contain("5 (not inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
	}

	[Test]
	public void AssertArgument_Open_Contained_NoException()
	{
		var subject = ValueRange.Open(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 3), Throws.Nothing);
	}

	[Test]
	public void AssertArgument_HalfOpen_NotContained_Exception()
	{
		var subject = ValueRange.HalfOpen(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 6),
			Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("[1..5)").And
				.With.Message.Contain("1 (inclusive)").And
				.With.Message.Contain("5 (not inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
	}

	[Test]
	public void AssertArgument_HalfOpen_Contained_NoException()
	{
		var subject = ValueRange.HalfOpen(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
	}

	[Test]
	public void AssertArgument_HalfClosed_NotContained_Exception()
	{
		var subject = ValueRange.HalfClosed(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 6),
			Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("(1..5]").And
				.With.Message.Contain("1 (not inclusive)").And
				.With.Message.Contain("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6));
	}

	[Test]
	public void AssertArgument_HalfClosed_Contained_NoException()
	{
		var subject = ValueRange.HalfClosed(1, 5);

		Assert.That(() => subject.AssertArgument("arg", 4), Throws.Nothing);
	}

	[Test]
	public void AssertArgument_NullCollection_Exception()
	{
		Assert.That(() => ValueRange.Closed(1, 5).AssertArgument("arg", null!),
			Throws.InstanceOf<ArgumentNullException>());
	}

	[Test]
	public void AssertArgument_AllContained_NoException()
	{
		Assert.That(() => ValueRange.Closed(1, 5).AssertArgument("arg", [2, 3, 4]),
			Throws.Nothing);
	}

	[Test]
	public void AssertArgument_SomeNotContained_ExceptionWithOffendingMember()
	{
		Assert.That(() => ValueRange.Closed(1, 5).AssertArgument("arg", [2, 6, 4]),
			Throws.InstanceOf<ArgumentOutOfRangeException>()
				.With.Message.Contain("[1..5]").And
				.With.Message.Contain("1 (inclusive)").And
				.With.Message.Contain("5 (inclusive)")
				.With.Property("ParamName").EqualTo("arg").And
				.With.Property("ActualValue").EqualTo(6)
		);
	}

	#endregion

	#endregion

	#region representation

	[Test]
	public void ToString_Closed_BetweenBrackets()
	{
		ValueRange<int> subject = ValueRange.Closed(1, 5);
		Assert.That(subject.ToString(), Is.EqualTo("[1..5]"));
	}

	[Test]
	public void ToString_Open_BetweenParenthesis()
	{
		ValueRange<int> subject = ValueRange.Open(1, 5);
		Assert.That(subject.ToString(), Is.EqualTo("(1..5)"));
	}

	[Test]
	public void ToString_HalfOpen_ParenthesisAtTheEnd()
	{
		ValueRange<int> subject = ValueRange.HalfOpen(1, 5);
		Assert.That(subject.ToString(), Is.EqualTo("[1..5)"));
	}

	[Test]
	public void ToString_HalfClosed_ParenthesisAtTheBeginning()
	{
		ValueRange<int> subject = ValueRange.HalfClosed(1, 5);
		Assert.That(subject.ToString(), Is.EqualTo("(1..5]"));
	}

	[Test, SetCulture("da-DK")]
	public void ToString_InvariantFormatting()
	{
		DateOnly lower = new(2025, 3, 1), upper = new(2025, 3, 2);
		
		Assert.That(lower.ToString(), Is.EqualTo("01.03.2025"), "day.month.year");
		
		ValueRange<DateOnly> subject = ValueRange.New(lower, upper);
		Assert.That(subject.ToString(), Does.StartWith("[03/01/2025..").And
			.EndsWith("..03/02/2025]"), "month/day/year");
	}
	
	[Test, SetCulture("da-DK")]
	public void ToString_CustomFormatting()
	{
		DateOnly lower = new(2025, 3, 1), upper = new(2025, 3, 2);
		
		Assert.That(lower.ToString(), Is.EqualTo("01.03.2025"), "day.month.year");
		
		ValueRange<DateOnly> subject = ValueRange.New(lower, upper);
		Assert.That(subject.ToString("o", CultureInfo.InvariantCulture), Does.StartWith("[2025-03-01..").And
			.EndsWith("..2025-03-02]"), "year-month-day");
	}

	#endregion

	#region equality

	[Test]
	public void Equals_SameBounds_True()
	{
		var range = new ValueRange<DateTime>(DateTime.MinValue, DateTime.MaxValue);
		var other = new ValueRange<DateTime>(DateTime.MinValue, DateTime.MaxValue);

		Assert.That(range.Equals(other), Is.True);
		Assert.That(other.Equals(range), Is.True);
	}

	[Test]
	public void Equals_DifferentBounds_False()
	{
		var range = new ValueRange<int>(1, 5);
		var other = new ValueRange<int>(1, 3);

		Assert.That(range.Equals(other), Is.False);
		Assert.That(other.Equals(range), Is.False);
	}

	[Test]
	public void Equals_DifferentTypeOfBounds_False()
	{
		Assert.That(ValueRange.Closed(1, 5).Equals(ValueRange.Open(1, 5)), Is.False);
		Assert.That(ValueRange.Closed(1, 5).Equals(ValueRange.HalfOpen(1, 5)), Is.False);
		Assert.That(ValueRange.Closed(1, 5).Equals(ValueRange.HalfClosed(1, 5)), Is.False);
	}

	[Test]
	public void Equals_Null_False()
	{
		ValueRange<int>? nil = null;
		var oneToFive = ValueRange.New(1, 5);

		Assert.That(oneToFive.Equals(nil), Is.False);
	}

	#endregion

	public static ValueRange<int>[] OneToFives =>
	[
		ValueRange.Open(1, 5),
		ValueRange.Closed(1, 5),
		ValueRange.HalfOpen(1, 5),
		ValueRange.HalfClosed(1, 5)
	];

	#region Contains

	[Test]
	public void Contains_WellContained_True([ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.Contains(3), Is.True);
	}

	[Test]
	public void Contains_NotContained_False([ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.Contains(6), Is.False);
	}

	[Test]
	public void Contains_CanBeUsedWithDates()
	{
		var ww2Period = ValueRange.Closed(3.September(1939), 2.September(1945));

		Assert.That(ww2Period.Contains(1.January(1940)), Is.True);
		Assert.That(ww2Period.Contains(1.January(1980)), Is.False);

		Assert.That(ww2Period.Contains(2.September(1939).At(12, 59, 59, 999)), Is.False,
			"In our definition, the beginning of the day is the upper bound");
	}

	#region lower bound item

	[Test]
	public void Contains_Closed_LowerBound_True()
	{
		var subject = ValueRange.Closed(1, 5);

		Assert.That(subject.Contains(1), Is.True);
	}

	[Test]
	public void Contains_Open_LowerBound_False()
	{
		var subject = ValueRange.Open(1, 5);

		Assert.That(subject.Contains(1), Is.False);
	}

	[Test]
	public void Contains_HalfOpen_LowerBound_True()
	{
		var subject = ValueRange.HalfOpen(1, 5);

		Assert.That(subject.Contains(1), Is.True);
	}

	[Test]
	public void Contains_HalfClosed_LowerBound_False()
	{
		var subject = ValueRange.HalfClosed(1, 5);

		Assert.That(subject.Contains(1), Is.False);
	}

	#endregion

	#region upper bound item

	[Test]
	public void Contains_Closed_UpperBound_True()
	{
		var subject = ValueRange.Closed(1, 5);

		Assert.That(subject.Contains(5), Is.True);
	}

	[Test]
	public void Contains_Open_UpperBound_False()
	{
		var subject = ValueRange.Open(1, 5);

		Assert.That(subject.Contains(5), Is.False);
	}

	[Test]
	public void Contains_HalfOpen_UpperBound_False()
	{
		var subject = ValueRange.HalfOpen(1, 5);

		Assert.That(subject.Contains(5), Is.False);
	}

	[Test]
	public void Contains_HalfClosed_UpperBound_True()
	{
		var subject = ValueRange.HalfClosed(1, 5);

		Assert.That(subject.Contains(5), Is.True);
	}

	#endregion

	[Test]
	public void Contains_Degenerate_ContainsValue()
	{
		TimeSpan value = 2.Seconds();
		var degenerate = ValueRange.Degenerate(value);

		Assert.That(degenerate.Contains(value), Is.True);
	}

	#endregion

	#region Intersect

	[Test]
	public void Intersect_Null_Empty()
	{
		ValueRange<int> notEmpty = ValueRange.New(1, 10);

		var intersection = notEmpty.Intersect(null);
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<int>()));
	}

	[Test]
	public void Intersect_NotEmptyToEmpty_Empty()
	{
		ValueRange<int> notEmpty = ValueRange.New(1, 10);

		var intersection = notEmpty.Intersect(ValueRange.Empty<int>());
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<int>()));
	}

	[Test]
	public void Intersect_EmptyToNull_Empty()
	{
		var intersection = ValueRange<byte>.Empty.Intersect(null);
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<byte>()));
	}

	[Test]
	public void Intersect_EmptyToNotEmpty_Empty()
	{
		ValueRange<int> notEmpty = ValueRange.New(1, 10);
		var intersection = ValueRange<int>.Empty.Intersect(notEmpty);
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<int>()));
	}

	[Test]
	public void Intersect_EmptyToEmpty_Empty()
	{
		var intersection = ValueRange<byte>.Empty.Intersect(ValueRange.Empty<byte>());
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<byte>()));
	}

	[Test]
	public void Intersect_WellDisjoint_Empty()
	{
		ValueRange<int> oneToTen = ValueRange.New(1, 10),
			fiftytoHundred = ValueRange.New(50, 100);

		var intersection = oneToTen.Intersect(fiftytoHundred);
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<int>()));
	}

	[Test]
	public void Intersect_LowerBoundNotTouchedByUpperBound_Empty()
	{
		ValueRange<int> open = ValueRange.Open(5, 10);
		ValueRange<int> closed = ValueRange.Closed(1, 5);

		var intersection = open.Intersect(closed);
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<int>()));
	}

	[Test]
	public void Intersect_UpperBoundNotTouchedByLowerBound_Empty()
	{
		ValueRange<int> closed = ValueRange.HalfOpen(5, 10);
		ValueRange<int> halfClosed = ValueRange.HalfClosed(10, 15);

		var intersection = closed.Intersect(halfClosed);
		Assert.That(intersection, Is.EqualTo(ValueRange.Empty<int>()));
	}

	[Test]
	public void Intersect_LowerBoundTouchingUpperBound_ClosedSingleLower()
	{
		ValueRange<int> halfOpen = ValueRange.HalfOpen(5, 10);
		ValueRange<int> halfClosed = ValueRange.HalfClosed(1, 5);

		var intersection = halfOpen.Intersect(halfClosed);

		Assert.That(intersection.Lower, Is.EqualTo(5));
		Assert.That(intersection.Upper, Is.EqualTo(5));
		Assert.That(intersection.Contains(5), Is.True);
	}

	[Test]
	public void Intersect_UpperBoundTouchingLowerBound_ClosedSingleUpper()
	{
		ValueRange<int> halfClosed = ValueRange.HalfClosed(5, 10);
		ValueRange<int> halfOpen = ValueRange.HalfOpen(10, 15);

		var intersection = halfClosed.Intersect(halfOpen);

		Assert.That(intersection.LowerBound, Is.EqualTo(Bound.Closed(10)));
		Assert.That(intersection.UpperBound, Is.EqualTo(Bound.Closed(10)));
		Assert.That(intersection.Contains(10), Is.True);
	}

	[Test]
	public void Intersect_WellContained_Contained()
	{
		ValueRange<int> container = ValueRange.New(1, 10),
			contained = ValueRange.New(3, 6);

		var intersection = container.Intersect(contained);

		Assert.That(intersection, Is.EqualTo(contained));
	}

	[Test]
	public void Intersect_CleanIntersection_MaxLowerMinUpper()
	{
		ValueRange<int> left = ValueRange.Closed(1, 5),
			right = ValueRange.Closed(3, 8);

		var intersection = left.Intersect(right);

		Assert.That(intersection.LowerBound, Is.EqualTo(Bound.Closed(3)));
		Assert.That(intersection.UpperBound, Is.EqualTo(Bound.Closed(5)));
	}

	[Test]
	public void Intersect_CleanIntersection_LowerNatureAsOfMaxLower()
	{
		ValueRange<int> left = ValueRange.Closed(1, 5),
			open = ValueRange.Open(3, 8),
			closed = ValueRange.Closed(3, 8);

		var openLower = left.Intersect(open);
		Assert.That(openLower.Contains(3), Is.False);

		var closedLower = left.Intersect(closed);
		Assert.That(closedLower.Contains(3), Is.True);
	}

	[Test]
	public void Intersect_CleanIntersection_UpperNatureAsOfMinUpper()
	{
		ValueRange<int> right = ValueRange.Closed(3, 8),
			open = ValueRange.Open(1, 5),
			closed = ValueRange.Closed(1, 5);

		var openUpper = right.Intersect(open);
		Assert.That(openUpper.Contains(5), Is.False);

		var closedUpper = right.Intersect(closed);
		Assert.That(closedUpper.Contains(3), Is.True);
	}

	#endregion

	#region Join

	[Test]
	public void Join_Null_Self()
	{
		ValueRange<int> range = ValueRange.New(1, 3);

		ValueRange<int> union = range.Join(null);

		Assert.That(union, Is.EqualTo(range));
	}

	[Test]
	public void Join_NotEmptyToEmpty_Self()
	{
		ValueRange<int> notEmpty = ValueRange.New(1, 3);

		var union = notEmpty.Join(ValueRange.Empty<int>());
		Assert.That(union, Is.EqualTo(notEmpty));
	}

	[Test]
	public void Join_EmptyToNotEmpty_NotEmpty()
	{
		ValueRange<int> notEmpty = ValueRange.New(1, 3);

		var union = ValueRange.Empty<int>().Join(notEmpty);

		Assert.That(union, Is.EqualTo(notEmpty));
	}

	[Test]
	public void Join_EmptyToNull_Empty()
	{
		var union = ValueRange.Empty<int>().Join(null);

		Assert.That(union, Is.EqualTo(ValueRange.Empty<int>()));
	}

	[Test]
	public void Join_EmptyToEmpty_Empty()
	{
		var union = ValueRange.Empty<byte>().Join(ValueRange<byte>.Empty);

		Assert.That(union, Is.EqualTo(ValueRange.Empty<byte>()));
	}

	[Test]
	public void Join_Disjointed_BigRange()
	{
		ValueRange<int> oneToThree = ValueRange.New(1, 3),
			nineToTen = ValueRange.New(9, 10);

		ValueRange<int> oneToSeven = oneToThree.Join(nineToTen);

		Assert.That(oneToSeven.LowerBound, Is.EqualTo(Bound.Closed(1)));
		Assert.That(oneToSeven.UpperBound, Is.EqualTo(Bound.Closed(10)));
	}

	[Test]
	public void Join_Intersecting_BigRange()
	{
		ValueRange<int> oneToThree = ValueRange.New(1, 3),
			twoToFive = ValueRange.New(2, 5);

		ValueRange<int> oneToFive = oneToThree.Join(twoToFive);

		Assert.That(oneToFive.LowerBound, Is.EqualTo(Bound.Closed(1)));
		Assert.That(oneToFive.UpperBound, Is.EqualTo(Bound.Closed(5)));
	}

	[Test]
	public void Join_Same_SameRange()
	{
		ValueRange<int> oneToThree = ValueRange.New(1, 3),
			anotherOneToThree = ValueRange.New(1, 3);

		ValueRange<int> oneToFive = oneToThree.Join(anotherOneToThree);

		Assert.That(oneToFive.LowerBound, Is.EqualTo(Bound.Closed(1)));
		Assert.That(oneToFive.UpperBound, Is.EqualTo(Bound.Closed(3)));
	}

	[Test]
	public void Join_LowerBound_IsSameBoundTypeAsMin()
	{
		ValueRange<int> oneToThree = ValueRange.Closed(1, 3),
			twoToFive = ValueRange.Open(2, 5);

		var oneToFive = oneToThree.Join(twoToFive);
		Assert.That(oneToFive.Contains(1), Is.True);
	}

	[Test]
	public void Join_UpperBound_IsSameBoundTypeAsMax()
	{
		ValueRange<int> oneToThree = ValueRange.Closed(1, 3),
			twoToFive = ValueRange.Open(2, 5);

		var oneToFive = oneToThree.Join(twoToFive);
		Assert.That(oneToFive.Contains(5), Is.False);
	}

	[Test]
	public void Join_LowerBoundTypeMissmatch_Closed()
	{
		ValueRange<int> oneToThree = ValueRange.HalfOpen(1, 3),
			oneToFive = ValueRange.HalfClosed(1, 5);

		var union = oneToThree.Join(oneToFive);

		Assert.That(union.Contains(1), Is.True);
	}

	[Test]
	public void Join_UpperBoundTypeMissmatch_Closed()
	{
		ValueRange<int> oneToThree = ValueRange.HalfOpen(1, 3),
			oneToFive = ValueRange.HalfClosed(1, 5);

		var union = oneToThree.Join(oneToFive);

		Assert.That(union.Contains(5), Is.True);
	}

	#endregion

	#region Overlaps

	[Test]
	public void Overlaps_NullOrEmpty_False()
	{
		var range = ValueRange.New(1, 2);

		Assert.That(range.Overlaps(null), Is.False);
		Assert.That(range.Overlaps(ValueRange.Empty<int>()), Is.False);
	}

	[Test]
	public void Overlaps_EmptyWithAnything_False()
	{
		var notEmpty = ValueRange.New<byte>(1, 2);

		Assert.That(ValueRange<byte>.Empty.Overlaps(null), Is.False);
		Assert.That(ValueRange<byte>.Empty.Overlaps(ValueRange.Empty<byte>()), Is.False);
		Assert.That(ValueRange<byte>.Empty.Overlaps(notEmpty), Is.False);
	}

	[Test]
	public void Overlaps_WellDisjoint_False()
	{
		ValueRange<int> oneToTen = ValueRange.New(1, 10),
			fiftytoHundred = ValueRange.New(50, 100);

		var overlapping = oneToTen.Overlaps(fiftytoHundred);
		Assert.That(overlapping, Is.False);
	}

	[Test]
	public void Overlaps_LowerBoundNotTouchedByUpperBound_False()
	{
		ValueRange<int> open = ValueRange.Open(5, 10);
		ValueRange<int> closed = ValueRange.Closed(1, 5);

		var overlapping = open.Overlaps(closed);
		Assert.That(overlapping, Is.False);
	}

	[Test]
	public void Overlaps_UpperBoundNotTouchedByLowerBound_False()
	{
		ValueRange<int> closed = ValueRange.HalfOpen(5, 10);
		ValueRange<int> halfClosed = ValueRange.HalfClosed(10, 15);

		var overlapping = closed.Overlaps(halfClosed);
		Assert.That(overlapping, Is.False);
	}

	[Test]
	public void Overlaps_LowerBoundTouchingUpperBound_True()
	{
		ValueRange<int> halfOpen = ValueRange.HalfOpen(5, 10);
		ValueRange<int> halfClosed = ValueRange.HalfClosed(1, 5);

		var overlapping = halfOpen.Overlaps(halfClosed);

		Assert.That(overlapping, Is.True);
	}

	[Test]
	public void Overlaps_UpperBoundTouchingLowerBound_True()
	{
		ValueRange<int> halfClosed = ValueRange.HalfClosed(5, 10);
		ValueRange<int> halfOpen = ValueRange.HalfOpen(10, 15);

		var overlapping = halfClosed.Overlaps(halfOpen);

		Assert.That(overlapping, Is.True);
	}


	[Test]
	public void Overlaps_WellContained_True()
	{
		ValueRange<int> container = ValueRange.New(1, 10),
			contained = ValueRange.New(3, 6);

		var overlapping = container.Overlaps(contained);

		Assert.That(overlapping, Is.True);
	}

	[Test]
	public void Overlaps_CleanIntersection_True()
	{
		ValueRange<int> left = ValueRange.Closed(1, 5),
			right = ValueRange.Closed(3, 8);

		var overlapping = left.Overlaps(right);

		Assert.That(overlapping, Is.True);
	}

	#endregion

	#region LimitLower

	[Test]
	public void LimitLower_WellContained_SameValue(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.LimitLower(3), Is.EqualTo(3));
	}

	[Test]
	public void LimitLower_NotContained_LimitAppliedOnlyInLowerEnd(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.LimitLower(0), Is.EqualTo(1));
		Assert.That(oneToFive.LimitLower(6), Is.EqualTo(6));
	}

	[Test]
	public void LimitLower_Bounds_SameValueRegardlessOfBoundInclusion(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.LimitLower(1), Is.EqualTo(1));
		Assert.That(oneToFive.LimitLower(5), Is.EqualTo(5));
	}

	#endregion

	#region LimitUpper

	[Test]
	public void LimitUpper_WellContained_SameValue(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.LimitUpper(3), Is.EqualTo(3));
	}

	[Test]
	public void LimitUpper_NotContained_LimitAppliedOnlyInUpperEnd(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.LimitUpper(0), Is.EqualTo(0));
		Assert.That(oneToFive.LimitUpper(6), Is.EqualTo(5));
	}

	[Test]
	public void LimitUpper_Bounds_SameValueRegardlessOfBoundInclusion(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.LimitUpper(1), Is.EqualTo(1));
		Assert.That(oneToFive.LimitUpper(5), Is.EqualTo(5));
	}

	#endregion

	#region Limit

	[Test]
	public void Limit_Contained_SameValue(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.Limit(3), Is.EqualTo(3));
	}

	[Test]
	public void Limit_NotContained_LimitLowerOrHigher(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.Limit(0), Is.EqualTo(1));
		Assert.That(oneToFive.Limit(6), Is.EqualTo(5));
	}

	[Test]
	public void Limit_Bounds_SameValueRegardlessOfBoundInclusion(
		[ValueSource(nameof(OneToFives))] ValueRange<int> oneToFive)
	{
		Assert.That(oneToFive.Limit(1), Is.EqualTo(1));
		Assert.That(oneToFive.Limit(5), Is.EqualTo(5));
	}

	#endregion

	#region Generate(next)

	[Test]
	public void Generate_ClosedPlusOne_YieldedAllItemsInRange()
	{
		Func<int, int> plusOne = i => i + 1;

		Assert.That(ValueRange.Closed(1, 5).Generate(plusOne), Is.EqualTo([1, 2, 3, 4, 5]));
		Assert.That(ValueRange.Closed(-5, -3).Generate(plusOne), Is.EqualTo([-5, -4, -3]));
		Assert.That(ValueRange.Closed(-2, 2).Generate(plusOne), Is.EqualTo([-2, -1, 0, 1, 2]));
	}

	[Test]
	public void Generate_OpenPlusTwo_BoundsNotYielded()
	{
		Assert.That(ValueRange.Open(1, 5).Generate(i => i + 2), Is.EqualTo([3]));

		Assert.That(ValueRange.Open(4, 10).Generate(x => x + 2), Is.EqualTo([6, 8]));
	}

	[Test]
	public void Generate_HalfClosedPlusOneMonth_LowerBoundNotYielded()
	{
		DateTime begin = 3.September(1939), threeMonthsLater = begin.AddMonths(3);

		Assert.That(
			ValueRange.HalfClosed(begin, threeMonthsLater).Generate((Func<DateTime, DateTime>)_oneMonthIncrement),
			Is.EqualTo([3.October(1939), 3.November(1939), 3.December(1939)]));
		return;

		DateTime _oneMonthIncrement(DateTime d) => d.AddMonths(1);
	}

	[Test]
	public void Generate_HalfOpenPlusTen_UpperBoundNotYielded()
	{
		Assert.That(ValueRange.HalfOpen(0m, 50m).Generate(d => d + 10), Is.EqualTo([0m, 10m, 20m, 30m, 40m]));
	}

	[Test]
	public void Generate_BackwardsGenerator_Exception()
	{
		Func<int, int> minusOne = i => i - 1;
		Assert.That(() => ValueRange.Closed(-10, -1).Generate(minusOne).ToArray(), Throws.ArgumentException);
	}

	[Test]
	public void Generate_NoOpGenerator_Exception()
	{
		Func<int, int> noOp = i => i;
		Assert.That(() => ValueRange.Closed(-10, -1).Generate(noOp).ToArray(), Throws.ArgumentException);
	}

	#endregion

	#region Generate(value)

	[Test]
	public void Generate_Value_TypeWithSumOperator_ValuesYielded()
	{
		Assert.That(ValueRange.Closed(1, 5).Generate(1), Is.EqualTo([1, 2, 3, 4, 5]));

		Assert.That(ValueRange.HalfOpen(0m, 50m).Generate(10m), Is.EqualTo([0m, 10m, 20m, 30m, 40m]));
	}

	[Test]
	public void Generate_Value_TypeWithoutSumOperator_Exception()
	{
		DateTime begin = 3.September(1939), threeMonthsLater = begin.AddMonths(3);

		Assert.That(() => ValueRange.HalfClosed(begin, threeMonthsLater).Generate(threeMonthsLater),
			Throws.InstanceOf<InvalidOperationException>()
				.With.Message.Contain("operator Add is not defined"));
	}

	#endregion

	#region Generate(StringGenerator)

	[Test]
	public void Generate_Closed_StringGenerator_CollectionOfSuccesiveStrings()
	{
		Assert.That(ValueRange.Closed("<<koala>>", "<<koale>>").Generate(ValueRange.StringGenerator), Is.EqualTo(
		[
			"<<koala>>",
			"<<koalb>>",
			"<<koalc>>",
			"<<koald>>",
			"<<koale>>"
		]));

		Assert.That(ValueRange.Closed("1", "7").Generate(ValueRange.StringGenerator), Is.EqualTo(
			["1", "2", "3", "4", "5", "6", "7"]));
	}

	[Test]
	public void Generate_Open_StringGenerator_ObbeysBoundInclusion()
	{
		Assert.That(ValueRange.Open("1", "7").Generate(ValueRange.StringGenerator), Is.EqualTo(
			["2", "3", "4", "5", "6"]));
	}

	#endregion

	#region Empty

	[Test]
	public void Empty_ContainsValue_False()
	{
		ValueRange<int> empty = ValueRange<int>.Empty;

		Assert.That(empty.Contains(0), Is.False);
		Assert.That(empty.Contains(-1), Is.False);
		Assert.That(empty.Contains(int.MinValue), Is.False);
		Assert.That(empty.Contains(int.MaxValue), Is.False);
		Assert.That(empty.Contains(1), Is.False);
	}

	[Test]
	public void Empty_Generate_Empty()
	{
		Assert.That(ValueRange<int>.Empty.Generate(x => x + 1), Is.Empty);
		Assert.That(ValueRange<int>.Empty.Generate(1), Is.Empty);
	}

	[Test]
	public void Empty_LimitLower_SameValue()
	{
		ValueRange<int> empty = ValueRange<int>.Empty;
		Assert.That(empty.LimitLower(0), Is.EqualTo(0));
		Assert.That(empty.LimitLower(-1), Is.EqualTo(-1));
		Assert.That(empty.LimitLower(2), Is.EqualTo(2));
	}

	[Test]
	public void Empty_LimitUpper_SameValue()
	{
		ValueRange<int> empty = ValueRange<int>.Empty;
		Assert.That(empty.LimitUpper(0), Is.EqualTo(0));
		Assert.That(empty.LimitUpper(-1), Is.EqualTo(-1));
		Assert.That(empty.LimitUpper(2), Is.EqualTo(2));
	}

	[Test]
	public void Empty_Limit_SameValue()
	{
		ValueRange<int> empty = ValueRange<int>.Empty;
		Assert.That(empty.Limit(0), Is.EqualTo(0));
		Assert.That(empty.Limit(-1), Is.EqualTo(-1));
		Assert.That(empty.Limit(2), Is.EqualTo(2));
	}

	[Test]
	public void Empty_IsEmpty() => Assert.That(ValueRange<int>.Empty.IsEmpty, Is.True);

	[Test]
	public void Default_IsNotEmpty()
	{
		Assert.That(new ValueRange<int>().IsEmpty, Is.False);
		Assert.That(default(ValueRange<int>).IsEmpty, Is.False);
	}

	[Test]
	public void Empty_IsNotDefault()
	{
		Assert.That(ValueRange<int>.Empty, Is.Not.EqualTo(new ValueRange<int>()));
		Assert.That(ValueRange<int>.Empty, Is.Not.EqualTo(default(ValueRange<int>)));
	}

	[Test]
	public void Empty_DefaultBounds_NotIncluded() =>
		Assert.That(ValueRange.Empty<int>().ToString(), Is.EqualTo("(0..0)"));

	[Test]
	public void Empty_Equals_DefaultDegenerated_False()
	{
		Assert.That(ValueRange.Empty<int>().Equals(ValueRange.Degenerate(0)), Is.False);
		Assert.That(ValueRange.Degenerate(0).Equals(ValueRange.Empty<int>()), Is.False);
	}

	// effectively nothing is equal to Empty as creation of open singularity is not possible
	[Test]
	public void Empty_IsOneOfAKind() =>
		Assert.That(() => ValueRange.Open(0, 0), Throws.InstanceOf<ArgumentOutOfRangeException>());

	#endregion
}