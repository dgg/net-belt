using Dumpify;

using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility;

[TestFixture, Category("showcase"), Explicit("noisy")]
public class ChainOfResponsibilityShowcase
{
	// re-implementing "the chain" again
	class MoreThanFive : IResponsibleLink<int?>
	{
		public IResponsibleLink<int?>? Next { get; private set; }
		public IResponsibleLink<int?> Chain(IResponsibleLink<int?> next) => Next = next;

		public Handled<int?> Handle(int? context)
		{
			if (context > 5)
			{
				context.Dump("Handled context bigger than 5");
				return new Handled<int?>(context * 2);
			}

			return Next?.Handle(context) ?? Handled<int?>.Not;
		}
	}

	// easier to implement
	class MoreThanTen : ResponsibleLinkBase<int?>
	{
		protected override bool CanHandle(int? context) => context > 10;

		protected override int? DoHandle(int? context)
		{
			context.Dump("Handled context bigger than 10");
			return context * 3;
		}
	}

	[Test]
	public void FluentChaining()
	{
		// no need to subclass, delegates are equally fine
		var moreThanThirty = new ResponsibleLink<int?>(i => i > 30,
			i =>
			{
				i.Dump("Handled context more than 30");
				return i * 4;
			});
		IResponsibleLink<int?> chain = moreThanThirty
			.Chain(new MoreThanTen())
			.Chain(new MoreThanFive());

		Assert.That(chain, Is.InstanceOf<MoreThanFive>(),
			"chain is created 'backwards' (returned link  is last)");

		Handled<int?> handled = chain.Handle(7);
		handled.Dump("handled by first (last) link");

		Handled<int?> unhandled = chain.Handle(4);
		unhandled.Dump("not handled by any link");
	}

	[Test]
	public void NotSoFluentChaining()
	{
		var moreThanThirty = new ResponsibleLink<int?>(i => i > 30,
			i =>
			{
				i.Dump("Handled context more than 30");
				return i * 4;
			});
		
		IResponsibleLink<int?> chain = Chain.OfResponsibility(
			new MoreThanFive(),
			new MoreThanTen(),
			moreThanThirty);
		Assert.That(chain, Is.InstanceOf<MoreThanFive>(), 
			"chain is created in the other of parameter passing");
		Assert.That(chain.Next?.Next, Is.SameAs(moreThanThirty));

		Handled<int?> handled = chain.Handle(11);
		handled.Dump("handled by second link");
	}

	record Context<T>(string Provider, T Message);

	class AHandler : ResponsibleAsyncLinkBase<Context<object>, int?>
	{
		// some async (or sync) operation to check whether the context is provided by A
		protected override ValueTask<bool> CanHandle(Context<object> context, CancellationToken cancellationToken) =>
			ValueTask.FromResult(context.Provider.Equals("A", StringComparison.Ordinal));

		protected override async Task<int?> DoHandle(Context<object> context, CancellationToken cancellationToken)
		{
			"pretend it failed to hitt some external store (db, blob, ...)".Dump();
			await Task.Delay(TimeSpan.FromMilliseconds(300), cancellationToken);
			throw new Exception("Could not store message from context");
		}
	}
	
	[Test, CancelAfter(500)]
	public async Task Async(CancellationToken ct)
	{
		var aHandler = new AHandler();
		var bHandler = new ResponsibleAsyncLink<Context<object>, int?>(
			(ctx, _) => ValueTask.FromResult(ctx.Provider.Equals("B", StringComparison.Ordinal)),
			(ctx, _) => Task.FromResult(new int?(1)));
		
		var chain = Chain.OfAsyncResponsibility(aHandler, bHandler);
		
		var handled = await chain.Handle(new Context<object>("B", new object()), ct);
		
		handled.Dump("handled by second link");
	}
}