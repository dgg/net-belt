using Net.Belt.Collections;

namespace Net.Belt.Tests.Collections;

[TestFixture]
public class SmartEnumerableTester
{
	[Test]
	public void Enumerate_EmptyEnumerable_Empty()
	{
		var subject = new SmartEnumerable<string>(Enumerable.Empty<string>());

		Assert.That(subject, Is.Empty);
	}

	[Test]
	public void Enumerate_SingleEntry_TheFirstAndTheLastEntry()
	{
		var subject = new SmartEnumerable<string>(["x"]);

		Assert.That(subject, Is.EqualTo([
			new SmartEntry<string>(0, "x", IsFirst: true, IsLast: true)
		]));
	}

	[Test]
	public void Enumerate_MutiplEntries_FirstLastAndSomeInBetween()
	{
		var subject = new SmartEnumerable<string>(["first", "middle", "last"]);

		Assert.That(subject, Is.EqualTo([
			new SmartEntry<string>(0, "first", IsFirst: true, IsLast: false),
			new SmartEntry<string>(1, "middle", IsFirst: false, IsLast: false),
			new SmartEntry<string>(2, "last", IsFirst: false, IsLast: true)
		]));
	}
}