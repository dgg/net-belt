using Net.Belt.Patterns.ChainOfResponsibility;
using Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility;

[TestFixture]
public class InheritanceResponsibleLinkTester
{
	#region Chain
	
	[Test]
	public void Chain_SetsNextProps()
	{
		Link<object> root = new();
		var next = new Link<object>();
		var last = new Link<object>();

		IResponsibleLink<object> chained = root.Chain(next).Chain(last);
		
		Assert.That(root.Next, Is.SameAs(next));
		Assert.That(next.Next, Is.SameAs(last));
		Assert.That(last.Next, Is.Null);
		Assert.That(chained, Is.SameAs(last));
	}

	[Test]
	public void Chain_Async_SetsNextProps()
	{
		IntToStringAsyncLink root = new(1);
		var next = new IntToStringAsyncLink(2);
		var last = new IntToStringAsyncLink(3);

		IResponsibleAsyncLink<int, string> chained = root.Chain(next).Chain(last);
		
		Assert.That(root.Next, Is.SameAs(next));
		Assert.That(next.Next, Is.SameAs(last));
		Assert.That(last.Next, Is.Null);
		Assert.That(chained, Is.SameAs(last));
	}

	#endregion

	#region Handle
	
	[Test]
	public void Handle_ReferenceContextSomeCould_Handled()
	{
		string context = "context";
		var chain = new MaybeToUpper("a")
			.Chain(new MaybeToUpper("b"))
			.Chain(new MaybeToUpper("c"));
		
		var handled = chain.Handle(context);
		
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.EqualTo("CONTEXT"));
	}
	
	[Test]
	public void Handle_ReferenceContextNoneCould_Unhandled()
	{
		string context = "context";
		var chain = new MaybeToUpper("a")
			.Chain(new MaybeToUpper("b"));
		
		var unHandled = chain.Handle(context);
		
		Assert.That(unHandled.IsHandled, Is.False);
		Assert.That(unHandled.Context, Is.Null);
	}
	
	[Test]
	public void Handle_ValueContextSomeCould_Handled()
	{
		int context = 5;
		var chain = new MaybePlusOne(10)
			.Chain(new MaybePlusOne(9))
			.Chain(new MaybePlusOne(4));
		
		var handled = chain.Handle(context);
		
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context.HasValue, Is.True);
		Assert.That(handled.Context, Is.EqualTo(6));
	}
	
	[Test]
	public void Handle_ValueContextNoneCould_Unhandled()
	{
		int context = 5;
		var chain = new MaybePlusOne(10)
			.Chain(new MaybePlusOne(9));
		
		var unHandled = chain.Handle(context);
		
		Assert.That(unHandled.IsHandled, Is.False);
		Assert.That(unHandled.Context.HasValue, Is.False);
		Assert.That(unHandled.Context, Is.Null);
	}
	
	[Test]
	public void Handle_DifferentReturnNoneCould_Unhandled()
	{
		string context = "later";
		IResponsibleLink<string, DateTimeOffset?> chain = new MaybeNow(TimeProvider.System);
		
		Handled<DateTimeOffset?> unHandled = chain.Handle(context);
		
		Assert.That(unHandled.IsHandled, Is.False);
		Assert.That(unHandled.Context.HasValue, Is.False);
		Assert.That(unHandled.Context, Is.Null);
	}
	
	#endregion
	
	#region AsyncHandle

	[Test, CancelAfter(1000)]
	public async Task HandleAsync_None_Unhandled(CancellationToken ct)
	{
		var chain = new IntToStringAsyncLink(1)
			.Chain(new IntToStringAsyncLink(2));
		var unhandled = await chain.Handle(4, ct);
		
		Assert.That(unhandled.IsHandled, Is.False);
		Assert.That(unhandled.Context, Is.Null);
	}
	
	[Test]
	public async Task HandleAsync_Some_Handled()
	{
		var chain = new IntToStringAsyncLink(1)
			.Chain(new IntToStringAsyncLink(2));
		
		// no token by default
		var handled = await chain.Handle(2);
		
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.EqualTo("2"));
	}
	
	#endregion
}