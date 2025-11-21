using System.Globalization;

using Net.Belt.Patterns.ChainOfResponsibility;

namespace Net.Belt.Tests.Patterns.ChainOfResponsibility.Support;

internal class IntToStringLink(int contextToHandle) : ResponsibleLinkBase<int, string>
{
	protected override bool CanHandle(int context) => context == contextToHandle;

	protected override string DoHandle(int context) => context.ToString(CultureInfo.InvariantCulture);
}