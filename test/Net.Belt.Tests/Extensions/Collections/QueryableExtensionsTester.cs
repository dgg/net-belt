using Net.Belt.Collections;
using Net.Belt.Comparisons;
using Net.Belt.Extensions.Collections;
using Net.Belt.Tests.Extensions.Collections.Support;

namespace Net.Belt.Tests.Extensions.Collections;

[TestFixture]
public class QueryableExtensionsTester
{
	#region Paginate

	[Test]
	public void Paginate_EmptyInput_Empty()
	{
		IQueryable<int> emptyQuery = Array.Empty<int>().AsQueryable();
		Assert.That(emptyQuery.Paginate(new Pagination(1, 1)), Is.Empty);
	}

	[Test]
	public void Paginate_WithinBounds_PageOfData()
	{
		IQueryable<int> oneToTen = Enumerable.Range(1, 10).AsQueryable();

		Assert.That(oneToTen.Paginate(new Pagination(3, 2)), 
			Is.EqualTo([4, 5, 6]));
	}

	[Test]
	public void Paginate_OutsideBounds_Empty()
	{
		IQueryable<int> oneToTen = Enumerable.Range(1, 10).AsQueryable();

		Assert.That(oneToTen.Paginate(new Pagination(5, 3)), Is.Empty);
	}

	#endregion
	
	#region sorting

	[Test]
	public void Sort_NullDirection_Unordered()
	{
		var query = new[] { 2, 5, 1 }.AsQueryable();
		Direction? nil = null;
		Assert.That(query.Sort(nil), Is.EqualTo(query));
	}

	[Test]
	public void Sort_AscendingDirection_Ascending()
	{
		var query = new[] { 2, 5, 1 }.AsQueryable();
		Assert.That(query.Sort(Direction.Ascending), Is.EqualTo([1, 2, 5]));
	}

	[Test]
	public void Sort_DescendingDirection_Descending()
	{
		var query = new[] { 2, 5, 1 }.AsQueryable();
		Assert.That(query.Sort(Direction.Descending), Is.EqualTo([5, 2, 1]));
	}

	[Test]
	public void SortBy_SelectorNullDirection_Unordered()
	{
		OrderSubject s2_10 = new(2, 10), s1_7 = new(1, 7), s1_8 = new(1, 8);
		var query = new[] { s2_10, s1_7, s1_8 }.AsQueryable();
		Direction? nil = null;
		Assert.That(query.SortBy(s => s.I2, nil), Is.EqualTo(query));
	}

	[Test]
	public void SortBy_SelectorAscendingDirection_Ascending()
	{
		OrderSubject s2_10 = new(2, 10), s1_7 = new(1, 7), s1_8 = new(1, 8);
		var query = new[] { s2_10, s1_7, s1_8 }.AsQueryable();
		Assert.That(query.SortBy(s => s.I2, Direction.Ascending), 
			Is.EqualTo([s1_7, s1_8, s2_10]));
	}

	[Test]
	public void SortBy_SelectorDescendingDirection_Descending()
	{
		OrderSubject s2_10 = new(2, 10), s1_7 = new(1, 7), s1_8 = new(1, 8);
		var query = new[] { s2_10, s1_7, s1_8 }.AsQueryable();
		Assert.That(query.SortBy(s => s.I2, Direction.Descending), Is.EqualTo(new[] { s2_10, s1_8, s1_7 }));
	}

	#endregion
}