using System.Globalization;

namespace Net.Belt;

internal class Class1
{
	private readonly int _param;
	public Class1(int param)
	{
		_param = param;
	}

	public string MultiMethod(int number)
	{
		int calculated = _param + number;
		return privateMethod(calculated);
	}

	public static string SingleMethod(int number) => privateMethod(number);

	private static string privateMethod(int number) => number.ToString(CultureInfo.InvariantCulture);
}
