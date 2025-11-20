using Net.Belt.Patterns.ChainOfResponsibility;
using Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility;

[TestFixture]
public class ChainOfResponsibilityTester
{
	[Test]
	public void OfResponsibility_Empty_Exception() =>
		Assert.That(() => Chain.OfResponsibility<object>(), Throws.ArgumentException);

	[Test]
	public void OfResponsibility_One_BuildsChainOfOne()
	{
		var first = new Link<object>();
		IResponsibleLink<object> chain = Chain.OfResponsibility(first);

		Assert.That(chain, Is.SameAs(first));
		Assert.That(chain.Next, Is.Null);
	}

	[Test]
	public void OfResponsibility_Many_BuildsChainOfMany()
	{
		Link<object> first = new(), second = new(), third = new();
		
		var chain = Chain.OfResponsibility(first, second, third);
		
		Assert.That(chain, Is.SameAs(first));
		Assert.That(chain.Next, Is.SameAs(second));
		Assert.That(second.Next, Is.SameAs(third));
		Assert.That(third.Next, Is.Null);
	}
	
	[Test]
	public void OfResponsibility_DifferentHandledType_BuildsChainOfMany()
	{
		IntToStringLink first = new(1), second = new(2), third = new(3);
		
		IResponsibleLink<int, string> chain = Chain.OfResponsibility(first, second, third);
		
		Assert.That(chain, Is.SameAs(first));
		Assert.That(chain.Next, Is.SameAs(second));
		Assert.That(second.Next, Is.SameAs(third));
		Assert.That(third.Next, Is.Null);
	}
	
	[Test]
	public void OfAsyncResponsibility_SameHandledType_BuildsChainOfMany()
	{
		AsyncLink<object> first = new(), second = new(), third = new();
		
		IResponsibleAsyncLink<object> chain = Chain.OfAsyncResponsibility(first, second, third);
		
		Assert.That(chain, Is.SameAs(first));
		Assert.That(chain.Next, Is.SameAs(second));
		Assert.That(second.Next, Is.SameAs(third));
		Assert.That(third.Next, Is.Null);
	}
	
	[Test]
	public void OfAsyncResponsibility_DifferentHandledType_BuildsChainOfMany()
	{
		IntToStringAsyncLink first = new(1), second = new(2), third = new(3);
		
		IResponsibleAsyncLink<int, string> chain = Chain.OfAsyncResponsibility(first, second, third);
		
		Assert.That(chain, Is.SameAs(first));
		Assert.That(chain.Next, Is.SameAs(second));
		Assert.That(second.Next, Is.SameAs(third));
		Assert.That(third.Next, Is.Null);
	}
}