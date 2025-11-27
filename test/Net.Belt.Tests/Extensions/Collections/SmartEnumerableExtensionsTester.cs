using Net.Belt.Collections;
using Net.Belt.Extensions.Collections;

namespace Net.Belt.Tests.Extensions.Collections;

[TestFixture]
public class SmartEnumerableExtensionsTester
{
	[Test]
	public void AsSmartEnumerable_MultipleEntries_WrapsElements()
	{
		IEnumerable<string> original = ["first", "middle", "last"]; 
		
		IEnumerable<SmartEntry<string>> smart = original.AsSmartEnumerable();

		Assert.That(smart, Is.EqualTo([
			new SmartEntry<string>(0, "first", IsFirst: true, IsLast: false),
			new SmartEntry<string>(1, "middle", IsFirst: false, IsLast: false),
			new SmartEntry<string>(2, "last", IsFirst: false, IsLast: true)
		]));
	}
}