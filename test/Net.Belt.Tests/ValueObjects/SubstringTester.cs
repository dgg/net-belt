using Net.Belt.ValueObjects;

namespace Net.Belt.Tests.ValueObjects;

[TestFixture]
public class SubstringTester
{
	[Test]
	public void ctor_ValuedSubstring()
	{
		var subject = new Substring("substr");
		Assert.That(subject.HasValue, Is.True);
		Assert.That(subject.Value, Is.EqualTo("substr"));
	}
	
	[Test]
	public void default_EmptySubstring()
	{
		var subject = new Substring();
		Assert.That(subject.HasValue, Is.False);
		Assert.That(subject.Value, Is.Empty);
	}
	
	[Test]
	public void Empty_EmptySubstring()
	{
		var subject = Substring.Empty;
		Assert.That(subject.HasValue, Is.False);
		Assert.That(subject.Value, Is.Empty);
	}
}