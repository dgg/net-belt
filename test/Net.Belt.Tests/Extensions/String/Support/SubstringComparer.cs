using Net.Belt.ValueObjects;

namespace Net.Belt.Tests.Extensions.String.Support;

internal class SubstringComparer : IEqualityComparer<Substring>
{
	public bool Equals(Substring x, Substring y) =>
		x.HasValue == y.HasValue && StringComparer.Ordinal.Equals(x.Value, y.Value);

	public int GetHashCode(Substring obj) => HashCode.Combine(obj.HasValue, obj.Value);
}

internal static class SubstringExtensions
{
	extension(Substring subtr)
	{
		internal static SubstringComparer Comparer => new();
	}
}