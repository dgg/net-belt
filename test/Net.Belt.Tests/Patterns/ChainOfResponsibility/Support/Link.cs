using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class Link<T> : ResponsibleLinkBase<T>
{
	protected override bool CanHandle(T context) => true;

	protected override T DoHandle(T context) => context;
}