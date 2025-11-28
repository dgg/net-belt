using Net.Belt.ValueObjects;

namespace Net.Belt.Collections;

/// <summary>
/// Represents the results of a paginated query.
/// </summary>
public interface IPaginatedResults
{
	/// <summary>
	/// Gets the current page number.
	/// </summary>
	uint CurrentPage { get; }
	/// <summary>
	/// Gets the total number of results across all pages.
	/// </summary>
	uint TotalResults { get; }
	/// <summary>
	/// Gets the total number of pages.
	/// </summary>
	uint TotalPages { get; }
	/// <summary>
	/// Gets the range of record numbers included in the current page.
	/// </summary>
	ValueRange<uint> RecordNumbers { get; }
	/// <summary>
	/// Gets the page number as a string.
	/// </summary>
	string PageNumber { get; }
	/// <summary>
	/// Gets the pagination information used for this result set.
	/// </summary>
	Pagination Pagination { get; }
}

/// <summary>
/// Represents a concrete implementation of <see cref="IPaginatedResults"/> for a specific type of data.
/// </summary>
/// <typeparam name="T">The type of the results in the page.</typeparam>
public class PaginatedResults<T> : IPaginatedResults
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PaginatedResults{T}"/> class with empty results.
	/// </summary>
	public PaginatedResults()
	{
		PageOfResults = [];
		RecordNumbers = ValueRange.Empty<uint>();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PaginatedResults{T}"/> class with the specified pagination details.
	/// </summary>
	/// <param name="pageOfResults">The list of results for the current page.</param>
	/// <param name="totalResults">The total number of results across all pages.</param>
	/// <param name="pagination">The pagination information.</param>
	public PaginatedResults(IReadOnlyList<T> pageOfResults, uint totalResults, Pagination pagination)
	{
		Pagination = pagination;
		CurrentPage = pagination.PageNumber == 0 ? 1 : pagination.PageNumber;
		PageOfResults = pageOfResults;
		TotalResults = totalResults;
		TotalPages = pagination.PageCount(totalResults);
		uint lastRecord = 0;
		if (CurrentPage <= TotalPages)
		{
			lastRecord = pageNotComplete(pageOfResults, pagination)
				? Convert.ToUInt32(pagination.FirstRecord + pageOfResults.Count - 1)
				: pagination.LastRecord;
		}

		RecordNumbers = lastRecord > 0
			? ValueRange.New(pagination.FirstRecord, lastRecord)
			: ValueRange.New(0u, 0u);
	}

	private static bool pageNotComplete(IReadOnlyList<T> pageOfResults, Pagination pagination) =>
		pageOfResults.Count < pagination.PageSize;

	/// <summary>
	/// Gets the read-only list of results for the current page.
	/// </summary>
	public IReadOnlyList<T> PageOfResults { get; private set; }
	/// <inheritdoc />
	public uint CurrentPage { get; }
	/// <inheritdoc />
	public uint TotalResults { get; }
	/// <inheritdoc />
	public uint TotalPages { get; }
	/// <inheritdoc />
	public ValueRange<uint> RecordNumbers { get; }
	/// <inheritdoc />
	public string PageNumber => nameof(PageNumber);
	/// <inheritdoc />
	public Pagination Pagination { get; }

	/// <summary>
	/// Projects the current <see cref="PaginatedResults{T}"/> to a new <see cref="PaginatedResults{TTo}"/>
	/// by applying a mapping function to each item in the <see cref="PageOfResults"/>.
	/// </summary>
	/// <typeparam name="TTo">The type to project the results to.</typeparam>
	/// <param name="mapper">The function to map each item from <typeparamref name="T"/> to <typeparamref name="TTo"/>.</param>
	/// <returns>A new <see cref="PaginatedResults{TTo}"/> containing the projected results.</returns>
	public PaginatedResults<TTo> Project<TTo>(Func<T, TTo> mapper) =>
		new(PageOfResults.Select(mapper).ToArray(), TotalResults, Pagination);
}