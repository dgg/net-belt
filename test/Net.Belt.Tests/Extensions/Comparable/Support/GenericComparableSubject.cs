using System.Runtime.CompilerServices;

namespace Net.Belt.Tests.Extensions.Comparable.Support;

internal class GenericComparableSubject<T>(T inner) : IComparable<GenericComparableSubject<T>> where T : struct, IComparable<T>
{
	public T Inner => inner;

	public int CompareTo(GenericComparableSubject<T>? other) => Inner.CompareTo(other?.Inner ?? default);
}