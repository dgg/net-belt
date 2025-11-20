using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility;

[TestFixture]
public class DelegatedResponsibleLinkTester
{
	#region Chain

	[Test]
	public void Chain_SetsNextProps()
	{
		ResponsibleLink<object> root = new(_ => false, _ => null!);
		var next = new ResponsibleLink<object>(_ => false, _ => null!);
		var last = new ResponsibleLink<object>(_ => true, o => o);

		IResponsibleLink<object> chained = root.Chain(next).Chain(last);

		Assert.That(root.Next, Is.SameAs(next));
		Assert.That(next.Next, Is.SameAs(last));
		Assert.That(last.Next, Is.Null);
		Assert.That(chained, Is.SameAs(last));
	}

	[Test]
	public void Chain_Async_SetsNextProps()
	{
		ResponsibleAsyncLink<object> root = new((_, _) => ValueTask.FromResult(false), (_, _) => null!);
		var next = new ResponsibleAsyncLink<object>((_, _) => ValueTask.FromResult(false), (_, _) => null!);
		var last = new ResponsibleAsyncLink<object>((_, _) => ValueTask.FromResult(true), (o, _) => Task.FromResult(o));

		IResponsibleAsyncLink<object> chained = root.Chain(next).Chain(last);

		Assert.That(root.Next, Is.SameAs(next));
		Assert.That(next.Next, Is.SameAs(last));
		Assert.That(last.Next, Is.Null);
		Assert.That(chained, Is.SameAs(last));
	}

	#endregion
	
	#region Handle
	
	[Test]
	public void Handle_SomeCould_Handled()
	{
		int context = 5;
		var shouldNotHandle = new ResponsibleLink<int, string>(
			i => i > 9, 
			_ => string.Empty);
		var shouldHandle = new ResponsibleLink<int, string>(
			i => i == 5, 
			ctx => ctx.ToString("B"));
		
		IResponsibleLink<int, string> chain = shouldNotHandle.Chain(shouldHandle);
		
		var handled = chain.Handle(context);
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.EqualTo("101"), "binary 5");
	}
	
	[Test]
	public void Handle_NoneCould_unhandled()
	{
		int context = 5;
		var shouldNotHandle = new ResponsibleLink<int, string>(
			i => i > 9, 
			_ => string.Empty);
		var shouldHandle = new ResponsibleLink<int, string>(
			i => i >8, 
			ctx => ctx.ToString("B"));
		
		IResponsibleLink<int, string> chain = shouldNotHandle.Chain(shouldHandle);
		
		var handled = chain.Handle(context);
		Assert.That(handled.IsHandled, Is.False);
		Assert.That(handled.Context, Is.Null);
	}
	
	#endregion
	
	#region AsyncHandle

	[Test, CancelAfter(1000)]
	public async Task HandleAsync_None_Unhandled(CancellationToken ct)
	{
		var chain = new ResponsibleAsyncLink<int, string>(
				(ctx, token) => ValueTask.FromResult(ctx == 1),
				(ctx, token) => Task.FromResult(string.Empty))
			.Chain(new ResponsibleAsyncLink<int, string>(
				(ctx, token) => ValueTask.FromResult(ctx == 2),
				(ctx, token) => Task.FromResult(string.Empty)));
		
		var unhandled = await chain.Handle(4, ct);
		
		Assert.That(unhandled.IsHandled, Is.False);
		Assert.That(unhandled.Context, Is.Null);
	}
	
	[Test]
	public async Task HandleAsync_Some_Handled()
	{
		var chain = new ResponsibleAsyncLink<int, string>(
				(ctx, token) => ValueTask.FromResult(ctx == 1),
				(ctx, token) => Task.FromResult(string.Empty))
			.Chain(new ResponsibleAsyncLink<int, string>(
				(ctx, token) => ValueTask.FromResult(ctx == 2),
				(ctx, token) => Task.FromResult(ctx.ToString("B"))));
		
		// no token by default
		var handled = await chain.Handle(2);
		
		Assert.That(handled.IsHandled, Is.True);
		Assert.That(handled.Context, Is.EqualTo("10"));
	}
	
	#endregion
}