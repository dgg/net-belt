using System.ComponentModel;
using System.Numerics;

namespace Net.Belt;

/// <summary>
/// 
/// </summary>
public class Enumeration
{
	#region checking

	/// <summary>
	/// Gets a value indicating whether <typeparamref name="TEnum"/> represents an enumeration.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if the <typeparamref name="TEnum"/> represents an enumeration; otherwise, <c>false</c>.</returns>
	public static bool IsEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible =>
		typeof(TEnum).IsEnum;

	/// <summary>
	/// Throws an exception if <typeparamref name="TEnum"/> is not an enumeration
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <exception cref="ArgumentException">The <typeparamref name="TEnum"/> is not an enumeration</exception>
	public static void AssertEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (!IsEnum<TEnum>())
		{
			throw new ArgumentException($"Type '{typeof(TEnum).Name}' is not an enum.");
		}
	}

	#endregion

	#region definition

	#region check

	/// <summary>
	/// Returns a boolean telling whether an enumerated value, exists in a specified enumeration.
	/// </summary>
	/// <param name="value">The value in <typeparamref name="TEnum"/>.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if the enumerated value exists in a specified enumeration; <c>false</c> otherwise.</returns>
	public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, Enum =>
		Enum.IsDefined(value);

	/// <summary>
	/// Returns a boolean telling whether a given integral value exists in a specified enumeration.
	/// </summary>
	/// <param name="underlying">The underlying value in <typeparamref name="TEnum"/></param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TUnderlying">Integral type of the value.</typeparam>
	/// <returns><c>true</c> if a constant in <typeparamref name="TEnum"/> has a value equal to <paramref name="underlying"/>;
	/// otherwise, <c>false</c>.</returns>
	public static bool IsDefined<TEnum, TUnderlying>(TUnderlying underlying)
		where TEnum : struct, Enum
		where TUnderlying : struct, INumber<TUnderlying> =>
		Enum.IsDefined(typeof(TEnum), underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(byte underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, byte>(underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(sbyte underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, sbyte>(underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(ushort underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, ushort>(underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(short underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, short>(underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(uint underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, uint>(underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(int underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, int>(underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(ulong underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, ulong>(underlying);

	/// <inheritdoc cref="IsDefined{TEnum,TUnderlying}"/>
	public static bool IsDefined<TEnum>(long underlying) where TEnum : struct, Enum =>
		IsDefined<TEnum, long>(underlying);

	/// <summary>
	/// Returns a boolean telling whether a name as a string exists in a specified enumeration.
	/// </summary>
	/// <param name="name">The name of <typeparamref name="TEnum"/></param>
	/// <param name="ignoreCase">Whether the name has to be compared ignoring the case.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <remarks>Names being compared ignoring the casing, are checked using ordinal comparison (<see cref="StringComparer.OrdinalIgnoreCase"/>).</remarks>
	/// <returns><c>true</c> if the name is defined in <typeparamref name="TEnum"/>; otherwise, <c>false</c>.</returns>
	public static bool IsDefined<TEnum>(string name, bool ignoreCase = false) where TEnum : struct, Enum =>
		Enum.IsDefined(typeof(TEnum), name) ||
		(ignoreCase && Enum.GetNames<TEnum>().Contains(name, StringComparer.OrdinalIgnoreCase));

	#endregion

	#region assert

	/// <summary>
	/// Throws an exception if an enumerated value does not exist in a specified enumeration.
	/// </summary>
	/// <param name="value">The value in <typeparamref name="TEnum"/>.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <exception cref="ArgumentException">If the <paramref name="value"/> is not in <typeparamref name="TEnum"/></exception>
	public static void AssertDefined<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		if (!IsDefined(value))
		{
			throwNotDefined(value);
		}
	}

	/// <summary>
	/// Throws an exception if an enumerated value does not exist in a specified enumeration.
	/// </summary>
	/// <param name="underlying">The value or name of a constant in <typeparamref name="TEnum"/>.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TUnderlying">Integral type of the value.</typeparam>
	/// <exception cref="ArgumentException">If the <paramref name="underlying"/> is not in <typeparamref name="TEnum"/></exception>
	public static void AssertDefined<TEnum, TUnderlying>(TUnderlying underlying)
		where TEnum : struct, Enum
		where TUnderlying : struct, INumber<TUnderlying>
	{
		if (!IsDefined<TEnum, TUnderlying>(underlying))
		{
			throwNotDefined<TEnum, TUnderlying>(underlying);
		}
	}

	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(byte underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, byte>(underlying);
	
	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(sbyte underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, sbyte>(underlying);
	
	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(ushort underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, ushort>(underlying);
	
	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(short underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, short>(underlying);
	
	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(uint underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, uint>(underlying);
	
	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(int underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, int>(underlying);
	
	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(ulong underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, ulong>(underlying);
	
	/// <inheritdoc cref="AssertDefined{TEnum,TUnderlying}"/>
	public static void AssertDefined<TEnum>(long underlying) where TEnum : struct, Enum =>
		AssertDefined<TEnum, long>(underlying);
	
	/// <summary>
	/// Throws an exception if an enumerated value does not exist in a specified enumeration.
	/// </summary>
	/// <remarks>Names being compared ignoring the casing, are checked using ordinal comparison (<see cref="StringComparer.OrdinalIgnoreCase"/>).</remarks>
	/// <param name="name">The name in <typeparamref name="TEnum"/></param>
	/// <param name="ignoreCase">Whether the name has to be compared ignoring the case.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <exception cref="ArgumentException">If the <paramref name="name"/> is not in <typeparamref name="TEnum"/></exception>
	public static void AssertDefined<TEnum>(string name, bool ignoreCase = false) where TEnum : struct, Enum
	{
		if (!IsDefined<TEnum>(name, ignoreCase))
		{
			throwNotDeclared<TEnum>(name);
		}
	}

	private static void throwNotDefined<TEnum>(TEnum value)
	{
		throw new ArgumentException(
			$"The value of argument '{nameof(value)}' ({value}) is not defined for Enum type '{typeof(TEnum).Name}'.",
			nameof(value));
	}

	private static void throwNotDefined<TEnum, TUnderlying>(TUnderlying underlying)
	{
		throw new ArgumentException(
			$"The value of argument '{nameof(underlying)}' ({underlying}) is not defined for Enum type '{typeof(TEnum).Name}'.",
			nameof(underlying));
	}
	
	private static void throwNotDeclared<TEnum>(string name)
	{
		throw new ArgumentException(
			$"The value of argument '{nameof(name)}' ('{name}') is not declared for Enum type '{typeof(TEnum).Name}'.",
			nameof(name));
	}

	#endregion

	#endregion
}