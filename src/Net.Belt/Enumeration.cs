using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
	/// <param name="underlying">The value of a constant in <typeparamref name="TEnum"/>.</param>
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

	#region names

	/// <summary>
	/// Retrieves an array of the names of the constants in a specified enumeration type.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>A string array of the names of the constants in <typeparamref name="TEnum"/>.</returns>
	public static string[] GetNames<TEnum>() where TEnum : struct, Enum => Enum.GetNames<TEnum>();

	/// <summary>
	/// Retrieves the name of the constant in the specified enumeration type that has the specified value.
	/// </summary>
	/// <param name="value">The value of a particular enumerated constant.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>A string containing the name of the enumerated constant in <typeparamref name="TEnum"/> which value is <paramref name="value"/>.</returns>
	/// <exception cref="ArgumentException">If the <paramref name="value"/> is not in <typeparamref name="TEnum"/></exception>
	public static string GetName<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		string? name = Enum.GetName(value);
		if (name is null)
		{
			throwNotDefined(value);
		}

		return name!;
	}

	/// <summary>
	/// Retrieves the name of the constant in the specified enumeration type that has the specified value.
	/// </summary>
	/// <param name="underlying">The value of a constant in <typeparamref name="TEnum"/>.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TUnderlying">Integral type of the value.</typeparam>
	/// <returns>A string containing the name of the enumerated constant in <typeparamref name="TEnum"/> which value is <paramref name="underlying"/>.</returns>
	/// <exception cref="ArgumentException">If the <paramref name="underlying"/> is not in <typeparamref name="TEnum"/></exception>
	public static string GetName<TEnum, TUnderlying>(TUnderlying underlying)
		where TEnum : struct, Enum
		where TUnderlying : struct, INumber<TUnderlying>
	{
		string? name = Enum.GetName(typeof(TEnum), underlying);
		if (name is null)
		{
			throwNotDefined<TEnum, TUnderlying>(underlying);
		}

		return name!;
	}

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(byte underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, byte>(underlying);

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(sbyte underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, sbyte>(underlying);

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(ushort underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, ushort>(underlying);

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(short underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, short>(underlying);

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(uint underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, uint>(underlying);

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(int underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, int>(underlying);

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(ulong underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, ulong>(underlying);

	/// <inheritdoc cref="GetName{TEnum,TUnderlying}(TUnderlying)"/>
	public static string GetName<TEnum>(long underlying)
		where TEnum : struct, Enum =>
		GetName<TEnum, long>(underlying);

	/// <summary>
	/// Retrieves the name of the constant in the specified enumeration type that has the specified value.
	/// A return value indicates whether the retrieval succeeded.
	/// </summary>
	/// <param name="value">The value of a particular enumerated constant .</param>
	/// <param name="name">A string containing the name of the enumerated constant in <typeparamref name="TEnum"/> which value is <paramref name="value"/>.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="value"/> was retrieved successfully; otherwise, <c>false</c>.</returns>
	public static bool TryGetName<TEnum>(TEnum value, [NotNullWhen(true)] out string? name) where TEnum : struct, Enum
	{
		name = Enum.GetName(value);
		return name is not null;
	}

	/// <summary>
	/// Retrieves the name of the constant in the specified enumeration type that has the specified value.
	/// A return value indicates whether the retrieval succeeded.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant .</param>
	/// <param name="name">A string containing the name of the enumerated constant in <typeparamref name="TEnum"/> which value is <paramref name="underlying"/>.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TUnderlying">Integral type of the value.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was retrieved successfully; otherwise, <c>false</c>.</returns>
	public static bool TryGetName<TEnum, TUnderlying>(TUnderlying underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		where TUnderlying : struct, INumber<TUnderlying>
	{
		name = Enum.GetName(typeof(TEnum), underlying);
		return name is not null;
	}

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(byte underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, byte>(underlying, out name);

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(sbyte underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, sbyte>(underlying, out name);

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(ushort underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, ushort>(underlying, out name);

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(short underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, short>(underlying, out name);

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(uint underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, uint>(underlying, out name);

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(int underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, int>(underlying, out name);

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(ulong underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, ulong>(underlying, out name);

	/// <inheritdoc cref="TryGetName{TEnum,TUnderlying}"/>
	public static bool TryGetName<TEnum>(long underlying, [NotNullWhen(true)] out string? name)
		where TEnum : struct, Enum
		=> TryGetName<TEnum, long>(underlying, out name);

	#endregion

	/// <summary>
	/// Returns the underlying type of the specified enumeration.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>The underlying type of enumType.</returns>
	public static Type GetUnderlyingType<TEnum>() where TEnum : struct, Enum => Enum.GetUnderlyingType(typeof(TEnum));

	#region values

	/// <summary>
	/// Retrieves an array of the values of the constants in a specified enumeration type.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>An array that contains the values of the constants in <typeparamref name="TEnum"/>.</returns>
	public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum => Enum.GetValues<TEnum>();

	/// <summary>
	/// Gets the numeric value of an enumeration.
	/// </summary>
	/// <param name="value">The value of a particular enumerated constant.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TUnderlying">Integral type of the value.</typeparam>
	/// <returns>The numeric value of <paramref name="value"/> in <typeparamref name="TEnum"/></returns>
	public static TUnderlying GetValue<TEnum, TUnderlying>(TEnum value)
		where TEnum : struct, Enum
		where TUnderlying : struct, INumber<TUnderlying>
	{
		AssertDefined(value);

		return (TUnderlying)Convert.ChangeType(value, typeof(TUnderlying));
	}

	/// <summary>
	/// Gets the numeric value of an enumeration.
	/// A return value indicates whether the retrieval succeeded.
	/// </summary>
	/// <param name="value">The value of a particular enumerated constant.</param>
	/// <param name="underlying">The numeric value of <paramref name="value"/> in <typeparamref name="TEnum"/>.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TUnderlying">Integral type of the value.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was retrieved successfully; otherwise, <c>false</c>.</returns>
	public static bool TryGetValue<TEnum, TUnderlying>(TEnum value, [NotNullWhen(true)]out TUnderlying? underlying)
		where TEnum : struct, Enum
		where TUnderlying : struct, INumber<TUnderlying>
	{
		bool result = false;
		underlying = null;
		if (IsDefined(value))
		{
			try
			{
				underlying = (TUnderlying)Convert.ChangeType(value, typeof(TUnderlying));
				result = true;
			}
			// there is no Convert.CanChangeType :-(
			catch (InvalidCastException) { }
			catch (OverflowException) { }
		}

		return result;
	}

	#endregion
}