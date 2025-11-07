namespace Net.Belt.Tests.Comparisons.Support;

internal record ComparisonSubject(string Property1, int Property2, decimal Property3): IComparable<ComparisonSubject>
{
	public override string ToString() => Property1;

	public int CompareTo(ComparisonSubject? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (other is null)
		{
			return 1;
		}

		return string.Compare(Property1, other.Property1, StringComparison.Ordinal);
	}
	
	public static readonly ComparisonSubject One = new("one", 1, 1m);
	public static readonly ComparisonSubject Two = new("two", 2, 2m);
}