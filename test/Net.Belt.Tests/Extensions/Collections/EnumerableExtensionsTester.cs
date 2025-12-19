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

			Enumerable.Range(1, 4).ForEach(i =>
			{
				times += 1;
			});

			Assert.That(times, Is.EqualTo(4));
		}

		[Test]
		public void Selecting_GoesOverIntCounting_SameLengthAndComposable()
		{
			int times = 0;

			var oneToFour = Enumerable.Range(1, 4)
				.Selecting(i => times += 1);

			// enumeration performed by this assertion is important, otherwise, laziness will surprise the developer
			Assert.That(oneToFour, Is.EqualTo([1, 2, 3, 4]));
			Assert.That(times, Is.EqualTo(4));
		}

		[Test]
		public void Selecting_OnlyAction_IsLazy()
		{
			int times = 0;

			var oneToFour = Enumerable.Range(1, 4).Selecting(i => times += 1);

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

			Enumerable.Range(2, 4).For((i, idx) =>
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

			var twoToFour = Enumerable.Range(2, 4).Selecting((i, idx) =>
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

			var twoToFour = Enumerable.Range(2, 4).Selecting((i, idx) =>
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
			Enumerable.Range(1, 4).For((i, item) =>
			{
				acc += (int)item + i;
			}, [1u, 2u]);

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
}