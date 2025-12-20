using System.Linq.Expressions;

using Net.Belt.Collections;
using Net.Belt.Comparisons;

namespace Net.Belt.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IQueryable{T}"/> to enhance querying capabilities.
/// </summary>
public static class QueryableExtensions
{
	extension<T>(IQueryable<T> source)
	{
	/// <summary>
	/// Paginates the elements of an <see cref="IQueryable{T}"/> sequence.
	/// </summary>
	/// <param name="pagination">The <see cref="Pagination"/> object containing pagination details.</param>
	/// <returns>An <see cref="IQueryable{T}"/> containing the paginated elements.</returns>
		public IQueryable<T> Paginate(Pagination pagination) => source
			.Skip((int)pagination.FirstRecord - 1)
			.Take(pagination.PageSize);
		
		/// <summary>
		/// Sorts the elements of a sequence.
		/// </summary>
		/// <remarks>If no <paramref name="direction"/> is passed, the sequence remains unsorted.</remarks>
		/// <param name="direction">Direction of the sorting (ascending or descending).</param>
		/// <returns>An <see cref="IOrderedQueryable{T}"/> which elements are sorted if a direction was specified.</returns>
		public IOrderedQueryable<T> Sort(Direction? direction)
		{
			if (!direction.HasValue) return (IOrderedQueryable<T>)source;
			IOrderedQueryable<T> ordered = direction.Value == Direction.Ascending
				? source.Order()
				: source.OrderDescending();
			return ordered;
		}

		/// <summary>
		/// Sorts the elements of a sequence according to a key.
		/// </summary>
		/// <remarks>If no <paramref name="direction"/> is passed, the sequence remains unsorted.</remarks>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		/// <param name="direction">Direction of the sorting (ascending or descending).</param>
		/// <typeparam name="TKey">The type of the key returned by the function that is represented by <paramref name="keySelector"/>.</typeparam>
		/// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to a key.</returns>
		public IOrderedQueryable<T> SortBy<TKey>(Expression<Func<T, TKey>> keySelector, Direction? direction)
		{
			if (!direction.HasValue) return (IOrderedQueryable<T>)source;
			IOrderedQueryable<T> ordered = direction.Value == Direction.Ascending
				? source.OrderBy(keySelector)
				: source.OrderByDescending(keySelector);
			return ordered;
		}
	}
}