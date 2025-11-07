namespace Net.Belt.Tests.Comparisons.Support;

internal class Property2Comparer : IComparer<ComparisonSubject>
{
	public int Compare(ComparisonSubject? x, ComparisonSubject? y)
	{
		// null management would be needed for every comparer implementation
		/*if (ReferenceEquals(x, y))
		{
			return 0;
		}

		if (y is null)
		{
			return 1;
		}

		if (x is null)
		{
			return -1;
		}*/

		// forcing null mishaps
		return x!.Property2.CompareTo(y!.Property2);
	}
}