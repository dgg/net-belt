namespace Net.Belt.Collections;

/// <summary>
/// Represents pagination information, including page size and current page number.
/// </summary>
/// <param name="PageSize">The maximum number of items per page.</param>
/// <param name="PageNumber">The current page number (1-based).</param>
public readonly record struct Pagination(ushort PageSize, uint PageNumber)
{
	/// <summary>
	/// Gets the number of the first record on the current page.
	/// </summary>
	public uint FirstRecord { get => PageNumber == 0 ? 1 : ((PageNumber - 1) * PageSize) + 1; }

	/// <summary>
	/// Gets the number of the last record on the current page.
	/// </summary>
	public uint LastRecord { get => PageSize == 0 ? FirstRecord : FirstRecord + PageSize - 1; }

	/// <summary>
	/// Calculates the total number of pages based on the total count of items and the page size.
	/// </summary>
	/// <param name="totalCount">The total number of items.</param>
	/// <returns>The total number of pages.</returns>
	public uint PageCount(uint totalCount)
	{
		uint n = totalCount % PageSize;
		
		uint numberOfPages = n > 0 ?
			(totalCount / PageSize) + 1 :
			totalCount / PageSize;

		return numberOfPages;
	}
}