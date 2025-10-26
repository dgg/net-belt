namespace Net.Belt.Tests.Extensions.Comparable.Support;

internal class ComparableSubject(IComparable inner) : IComparable
{
	public int CompareTo(object? obj) => inner.CompareTo(obj);
}