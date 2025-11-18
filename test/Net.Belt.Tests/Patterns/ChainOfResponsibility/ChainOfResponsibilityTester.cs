using Net.Belt.Patterns.ChainOfResponsibility;
using Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility;

[TestFixture]
public class ChainOfResponsibilityTester
{
	[Test, Category("Exploratory")]
	public void Handled_ContextModified()
	{
		var chain = Chain.OfResponsibility<Context>()
			.Chain(new ToUpperIfStartsWith("1"))
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("2_a");
		chain.Handle(context);
		Assert.That(context.S, Is.EqualTo("2_A"));
	}
	[Test, Category("Exploratory")]
	public void Handled_ContextModified_()
	{
		var chain = new ToUpperIfStartsWith("1")
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("2_a");
		chain.Handle(context);
		Assert.That(context.S, Is.EqualTo("2_A"));
	}
	
	[Test, Category("Exploratory")]
	public void NotHandled_ContextNotModified()
	{
		var chain = Chain.OfResponsibility<Context>()
			.Chain(new ToUpperIfStartsWith("1"))
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("5_a");
		chain.Handle(context);
		Assert.That(context.S, Is.EqualTo("5_a"));
	}
	[Test, Category("Exploratory")]
	public void NotHandled_ContextNotModified_()
	{
		var chain = new ToUpperIfStartsWith("1")
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("5_a");
		chain.Handle(context);
		Assert.That(context.S, Is.EqualTo("5_a"));
	}
	
	[Test, Category("Exploratory")]
	public void TryHandled_ContextModified()
	{
		var chain = Chain.OfResponsibility<Context>()
			.Chain(new ToUpperIfStartsWith("1"))
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("2_a");
		Assert.That(chain.TryHandle(context), Is.True);
		Assert.That(context.S, Is.EqualTo("2_A"));
	}
	
	[Test, Category("Exploratory")]
	public void TryHandled_ContextModified_()
	{
		var chain = new ToUpperIfStartsWith("1")
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("2_a");
		Assert.That(chain.TryHandle(context), Is.True);
		Assert.That(context.S, Is.EqualTo("2_A"));
	}
	
	[Test, Category("Exploratory")]
	public void TryNotHandled_ContextNotModified()
	{
		var chain = Chain.OfResponsibility<Context>()
			.Chain(new ToUpperIfStartsWith("1"))
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("5_a");
		Assert.That(chain.TryHandle(context), Is.False);
		Assert.That(context.S, Is.EqualTo("5_a"));
	}
	
	[Test, Category("Exploratory")]
	public void TryNotHandled_ContextNotModified_()
	{
		var chain = new ToUpperIfStartsWith("1")
			.Chain(new ToUpperIfStartsWith("2"))
			.Chain(new ToUpperIfStartsWith("3"));

		var context = new Context("5_a");
		Assert.That(chain.TryHandle(context), Is.False);
		Assert.That(context.S, Is.EqualTo("5_a"));
	}
}