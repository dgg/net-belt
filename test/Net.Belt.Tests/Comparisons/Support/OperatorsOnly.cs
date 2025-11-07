using System.Globalization;

namespace Net.Belt.Tests.Comparisons.Support;

internal class OperatorsOnly(int value)
{
	private readonly int _value = value;

	public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);

	public static bool operator >(OperatorsOnly x, OperatorsOnly y) => x._value > y._value;

	public static bool operator <(OperatorsOnly x, OperatorsOnly y) => x._value < y._value;
}