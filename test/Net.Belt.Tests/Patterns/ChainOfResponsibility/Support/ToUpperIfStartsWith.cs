using System.Diagnostics.CodeAnalysis;

using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class ToUpperIfStartsWith(string substring) : ChainOfResponsibilityLink<Context>
{
	public override bool CanHandle(Context context) => context.S.StartsWith(substring);

	protected override void DoHandle(Context context) => context.S = context.S.ToUpperInvariant();
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal class IToUpperIfStartsWith(string substring) : IChainOfResponsibilityLink<Context>
{
	public bool CanHandle(Context context) => context.S.StartsWith(substring);

	public void DoHandle(Context context) => context.S = context.S.ToUpperInvariant();
}