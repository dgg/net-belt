using System.Diagnostics.CodeAnalysis;

using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class ToUpperIfStartsWith(string substring) : ResponsibleLinkBase<Context>
{
	protected override bool CanHandle(Context context) => context.S.StartsWith(substring);

	protected override Context DoHandle(Context context) => new(context.S.ToUpperInvariant());
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal class IToUpperIfStartsWith(string substring) : IResponsibleLink<Context>
{
	public IResponsibleLink<Context>? Next { get; private set; }
	public IResponsibleLink<Context> Chain(IResponsibleLink<Context> next) => Next = next;

	public Handled<Context> Handle(Context context) =>
		context.S.StartsWith(substring)
			? new Handled<Context>(new(context.S.ToUpperInvariant()))
			: Handled<Context>.Not;
}