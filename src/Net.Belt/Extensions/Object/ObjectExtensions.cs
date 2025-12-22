using System.Diagnostics;
using System.Numerics;

using Net.Belt.Extensions.Numerics;

namespace Net.Belt.Extensions.Object;

/// <summary>
/// 
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="maybeBool"></param>
	/// <returns></returns>
	public static bool? ConvertToBool(this string maybeBool)
	{
		if (bool.TryParse(maybeBool, out var result)) return result;
		StringComparer cmp = StringComparer.OrdinalIgnoreCase;
		if (cmp.Equals(maybeBool, "1") || cmp.Equals(maybeBool, "t")) return true;
		if (cmp.Equals(maybeBool, "0") || cmp.Equals(maybeBool, "f")) return false;
		return null;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="maybeBool"></param>
	/// <returns></returns>
	public static bool? ConvertToBool(this char maybeBool) =>
		maybeBool switch
		{
			'0' or 'f' or 'F' or char.MinValue => false,
			'1' or 't' or 'T' or (char)1 => true,
			_ => null
		};

	/// <summary>
	/// 
	/// </summary>
	/// <param name="maybeBool"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static bool? ConvertToBool<T>(T maybeBool) where T : INumber<T>
	{
		if (maybeBool.Equals(T.Zero)) return false;
		if (maybeBool.Equals(T.One)) return true;
		return null;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="maybeBool"></param>
	/// <returns></returns>
	public static bool? ConvertToBool(this object maybeBool)
	{
		bool? result = null;
		if (maybeBool is bool b) result = b;
		else if (maybeBool is char c) result = c.ConvertToBool();
		else if (maybeBool is string s) result = s.ConvertToBool();
		else if (maybeBool is BigInteger bi) result = ConvertToBool(bi);
		else if (maybeBool is Int128 ii) result = ConvertToBool(ii);
		else if (maybeBool is UInt128 ui) result = ConvertToBool(ui);
		else if (maybeBool is IntPtr ip) result = ConvertToBool(ip);
		else if (maybeBool is UIntPtr uip) result = ConvertToBool(uip);
		else if (maybeBool is Half h) result = ConvertToBool(h);
		// convertible numeric
		else
		{
			try
			{
				ulong ul = Convert.ToUInt64(maybeBool);
				if (ul.Equals(1UL))
				{
					result = true;
				} else if (ul.Equals(0UL))
				{
					result = false;
				}
			}
			catch
			{
				// swallow conversion exceptions for edge cases :-(
			}
		}

		return result;
	} 
}