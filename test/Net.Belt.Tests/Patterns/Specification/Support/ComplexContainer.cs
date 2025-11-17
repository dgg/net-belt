using System.Collections;

namespace Net.Belt.Tests.Patterns.Specification.Support;

internal class ComplexContainer : IEnumerable<ComplexType>
{
	private readonly List<ComplexType> _inner =
	[
		new(true, "12345", 5), new(false, "12345", 5),
		new(true, "12", 2), new(false, "12", 2),
		new(true, "12345", 2), new(false, "12345", 2),
		new(true, "12", 5), new(false, "12", 5)
	];

	public IEnumerator<ComplexType> GetEnumerator()
	{
		return _inner.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}