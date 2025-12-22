using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Net.Belt.Extensions.Collections;

namespace Net.Belt.Tests.Extensions.Collections;

[TestFixture]
public class EnumerableExtensionsTester
{
	#region nullability

	[Test]
	public void EmptyIfNull_Null_Empty()
	{
		IEnumerable<int>? nil = null;
		Assert.That(nil.EmptyIfNull, Is.Not.Null.And.Empty);
	}

	[Test]
	public void EmptyIfNull_Empty_StillEmpty()
	{
		IEnumerable<int> empty = [];
		Assert.That(empty.EmptyIfNull, Is.Empty);
	}

	[Test]
	public void EmptyIfNull_NotNull_Same()
	{
		IEnumerable<int> notNull = [1, 2, 3];
		Assert.That(notNull.EmptyIfNull, Is.SameAs(notNull));
	}

	[Test]
	public void SkipNulls_NullItems_NotEnumerated()
	{
		IEnumerable<string?> source = ["1", null, "2", "3"];
		Assert.That(source.SkipNulls(), Is.EqualTo(["1", "2", "3"]));
	}

	#endregion

	#region count constraints

	[Test]
	public void HasOne_OneElement_True() =>
		Assert.That(new[] { 1 }.Has.One, Is.True);

	[Test]
	public void HasOne_MoreThanOne_False() =>
		Assert.That(new[] { 1, 2 }.Has.One, Is.False);

	[Test]
	public void HasOne_LessThanOneElement_False() =>
		Assert.That(Enumerable.Empty<int>().Has.One, Is.False);

	[Test]
	public void HasAtLeast_Empty_HasAtMost0()
	{
		Assert.That(Enumerable.Empty<int>().Has.AtLeast(0), Is.True, "An empty collection has at least 0 elements");
		Assert.That(Enumerable.Empty<int>().Has.AtLeast(1), Is.False);
		Assert.That(Enumerable.Empty<DateTime>().Has.AtLeast(2), Is.False);
	}


	[TestCase(0u),
	 TestCase(1u),
	 TestCase(2u),
	 TestCase(3u),
	 TestCase(4u)]
	public void HasAtLeast_LessOrEqThanLength_True(uint length) =>
		Assert.That(Enumerable.Range(1, 4).Has.AtLeast(length), Is.True);


	[TestCase(5u), TestCase(6u)]
	public void HasAtLeast_MoreThanLength_False(uint length) =>
		Assert.That(Enumerable.Range(1, 4).Has.AtLeast(length), Is.False);

	#endregion

	#region enumeration

	[Test]
	public void Foreach_GoesOverIntCounting_SameLength()
	{
		int times = 0;

		Enumerable.Range(1, 4).ForEach(_ => { times += 1; });

		Assert.That(times, Is.EqualTo(4));
	}

	[Test]
	public void Selecting_GoesOverIntCounting_SameLengthAndComposable()
	{
		int times = 0;

		var oneToFour = Enumerable.Range(1, 4)
			.Selecting(_ => times += 1);

		// enumeration performed by this assertion is important, otherwise, laziness will surprise the developer
		Assert.That(oneToFour, Is.EqualTo([1, 2, 3, 4]));
		Assert.That(times, Is.EqualTo(4));
	}

	[Test]
	public void Selecting_OnlyAction_IsLazy()
	{
		int times = 0;

		var oneToFour = Enumerable.Range(1, 4).Selecting(_ => times += 1);

		Assert.That(times, Is.Zero,
			"Because we have not enumerated the collection");
		// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
		oneToFour.ToArray();
		Assert.That(times, Is.EqualTo(4),
			"Because the collection has been enumerated");
	}

	[Test]
	public void For_InvokesActionWithItemAndIndex()
	{
		int count = 0;

		Enumerable.Range(2, 4).For((i, _) =>
		{
			count += 1;
			Assert.That(i, Is.EqualTo(count + 1));
		});

		Assert.That(count, Is.EqualTo(4));
	}

	[Test]
	public void Selecting_InvokesActionWithItemAndIndex()
	{
		int count = 0;

		var twoToFour = Enumerable.Range(2, 4).Selecting((i, _) =>
		{
			count += 1;
			Assert.That(i, Is.EqualTo(count + 1));
		});

		Assert.That(twoToFour, Is.EqualTo([2, 3, 4, 5]),
			"enumerate first to increase counter");
		Assert.That(count, Is.EqualTo(4));
	}

	[Test]
	public void Selecting_WithIndex_IsLazy()
	{
		int count = 0;

		var twoToFour = Enumerable.Range(2, 4).Selecting((i, _) =>
		{
			count += 1;
			Assert.That(i, Is.EqualTo(count + 1));
		});

		Assert.That(count, Is.Zero,
			"Because we have not enumerated the collection");
		// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
		twoToFour.ToArray();
		Assert.That(count, Is.EqualTo(4),
			"Because the collection has been enumerated");
	}

	[Test]
	public void For_PerformsActionOnIndexes()
	{
		int acc = 0;
		Enumerable.Range(1, 4).For((i, item) => { acc += (int)item + i; }, [1u, 2u]);

		Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
	}

	[Test]
	public void For_PerformsActionOnIndexesThatCanBeSpecifiedAsOptionalParameters()
	{
		int acc = 0;
		Action<int, uint> action = (i, item) => acc += (int)item + i;
		Enumerable.Range(1, 4).For(action, [1u, 2u]);

		Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
	}

	[Test]
	public void Selecting_PerformsActionOnIndexesThatCanBeSpecifiedAsOptionalParameters()
	{
		int acc = 0;
		Action<int, uint> tally = (i, item) => acc += (int)item + i;
		var oneToFour = Enumerable.Range(1, 4).Selecting(tally, [1u, 2u]);

		Assert.That(oneToFour, Is.EqualTo([1, 2, 3, 4]),
			"enumerate before checking counter");
		Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)));
	}

	[Test]
	public void Selecting_OptionalIndexes_IsLazy()
	{
		int acc = 0;
		Action<int, uint> tally = (i, item) => acc += (int)item + i;
		var oneToFour = Enumerable.Range(1, 4).Selecting(tally, [1u, 2u]);

		Assert.That(acc, Is.EqualTo(0), "Because we have not enumerated the collection");
		// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
		oneToFour.ToArray();
		Assert.That(acc, Is.EqualTo((2 + 1) + (3 + 2)), "Because the collection has been enumerated");
	}

	#endregion

	#region ToDelimitedString

	[Test]
	public void ToDelimitedString_PopulatedCollections_DelimitedString()
	{
		var source = new[] { 1, 2, 3, 4 };
		Func<int, string> stringify = i => (i * 2).ToString(CultureInfo.InvariantCulture);

		Assert.That(source.ToDelimitedString(), Is.EqualTo("1, 2, 3, 4"));
		Assert.That(source.ToDelimitedString(toString: stringify), Is.EqualTo("2, 4, 6, 8"));
		Assert.That(source.ToDelimitedString("-"), Is.EqualTo("1-2-3-4"));
		Assert.That(source.ToDelimitedString("*", stringify), Is.EqualTo("2*4*6*8"));
	}

	[Test]
	public void ToDelimitedString_EmptyEnumerable_Empty()
	{
		Assert.That(Enumerable.Empty<string>().ToDelimitedString(), Is.Empty);
		Assert.That(Enumerable.Empty<string>().ToDelimitedString(toString: _ => string.Empty), Is.Empty);
		Assert.That(Enumerable.Empty<string>().ToDelimitedString("*"), Is.Empty);
		Assert.That(Enumerable.Empty<string>().ToDelimitedString("_", _ => string.Empty), Is.Empty);
	}

	#endregion

	#region generation

	[Test]
	public void Skipping_Zero_RegularIterator() =>
		Assert.That(new[] { 1, 2, 3 }.Gen.Skipping(0),
			Is.EqualTo([1, 2, 3]));

	[Test]
	public void Skipping_One_EveryOtherElement() =>
		Assert.That(new[] { 1, 2, 3, 4, 5 }.Gen.Skipping(1),
			Is.EqualTo([1, 3, 5]));

	[Test]
	public void Skipping_MoreThanOne_SkipsElements()
	{
		Assert.That(Enumerable.Range(1, 10).Gen.Skipping(2u), Is.EqualTo([1, 4, 7, 10]));
		Assert.That(Enumerable.Range(1, 10).Gen.Skipping(3u), Is.EqualTo([1, 5, 9]));
		Assert.That(Enumerable.Range(1, 10).Gen.Skipping(4u), Is.EqualTo([1, 6]));
	}

	[Test]
	public void Skipping_Three_SkipsThreeElements() =>
		Assert.That(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Gen.Skipping(3), Is.EqualTo([1, 5, 9]));


	[Test]
	public void Interlace_SameSizeEnumerations_AlternatesOneElementFromEach()
	{
		IEnumerable<int> odds = [1, 3, 5], evens = [2, 4, 6];

		IEnumerable<int> oneToSix = odds.Gen.Interlace(evens);
		Assert.That(oneToSix, Is.EqualTo([1, 2, 3, 4, 5, 6]));
	}

	[Test]
	public void Interlace_SomeIsEmpty_Empty()
	{
		Assert.That(Enumerable.Empty<int>().Gen.Interlace([1]), Is.Empty);
		Assert.That(new[] { 1 }.Gen.Interlace([]), Is.Empty);
	}

	[Test]
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public void Interlace_DifferentSizes_ShortestRules()
	{
		IEnumerable<int> odds = [1, 3, 5], evens = [2, 4];

		Assert.That(odds.Gen.Interlace(evens), Is.EqualTo([1, 2, 3, 4]));
		Assert.That(evens.Gen.Interlace(odds), Is.EqualTo([2, 1, 4, 3]));
	}

	[Test]
	public void Interlace_SampleWithWords()
	{
		var source = new[] { "The", "quick", "brown", "fox" };

		string result = source.Gen.Interlace(_streamOfSpaces())
			.Aggregate(string.Empty, (a, b) => a + b)
			.TrimEnd();

		Assert.That(result, Is.EqualTo("The quick brown fox"));
		return;

		[SuppressMessage("ReSharper", "IteratorNeverReturns")]
		IEnumerable<string> _streamOfSpaces()
		{
			while (true) yield return " ";
		}
	}
	
	[Test]
	public void InBatchesOf_BatchSizeIsFactorOfInputLength_NumberOfBacthes()
	{
		var result = new[]
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
			1, 2, 3, 4, 5, 6, 7, 8, 9, 0
		}.Gen.InBatchesOf(10);

		Assert.That(result, Is.EqualTo([
			new[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 0},
			[1, 2, 3, 4, 5, 6, 7, 8, 9, 0]
		]));
	}

	[Test]
	public void InBatchesOf_BatchSizeIsNotFactorOfInputLength_NumberOfBatchesPlusOne()
	{
		var result = new[]
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
			1, 2, 3, 4, 5, 6, 7, 8, 9, 0
		}.Gen.InBatchesOf(9);

		Assert.That(result, Is.EqualTo([
			new[] {1, 2, 3, 4, 5, 6, 7, 8, 9 },
			[0, 1, 2, 3, 4, 5, 6, 7, 8],
			[9, 0]
		]));
	}

	[Test]
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public void InBatchesOf_BatchSizeIsIsGreaterThanInputLength_OneBatchWithAllElements()
	{
		IEnumerable<int> input =
		[
			1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
			1, 2, 3, 4, 5, 6, 7, 8, 9, 0
		];

		Assert.That(input.Gen.InBatchesOf(1000), 
			Is.EqualTo([input]));
	}

	#endregion
}