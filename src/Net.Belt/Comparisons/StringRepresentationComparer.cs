namespace Net.Belt.Comparisons;

/// <summary>
/// Compares string representations of objects by converting them to a specified type and applying a comparison logic.
/// </summary>
/// <typeparam name="T">The type to which string representations will be converted during comparison.</typeparam>
/// <param name="converter">
/// A function delegate that converts a string representation to an instance of the specified type T for comparison.
/// </param>
/// <param name="direction">
/// Specifies the order in which the comparison should be performed.
/// Defaults to <see cref="Direction.Ascending"/>.
/// </param>
public class StringRepresentationComparer<T>(Func<string, T> converter, Direction direction = Direction.Ascending)
	: ChainableComparer<string>(direction)
{
	private readonly Comparer<T> _comparer = Comparer<T>.Default;

	/// <summary>
	/// Performs the comparison of two string representations by converting them to a specified type and comparing the resulting values.
	/// </summary>
	/// <param name="x">The first string representation to compare.</param>
	/// <param name="y">The second string representation to compare.</param>
	/// <returns>
	/// An integer that indicates the relative order of the two string representations:
	/// less than zero if <paramref name="x"/> is less than <paramref name="y"/>,
	/// zero if they are equal,
	/// or greater than zero if <paramref name="x"/> is greater than <paramref name="y"/>.
	/// </returns>
	protected override int DoCompare(string x, string y) => _comparer.Compare(converter(x), converter(y));
}