using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace Net.Belt;

/// <summary>
/// Provides helper methods for working with enumeration types (enum).
/// </summary>
public static class Enumeration
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

	[DoesNotReturn]
	private static void throwNotDefined<TEnum>(TEnum value)
	{
		throw new ArgumentException(
			$"The value of argument '{nameof(value)}' ({value}) is not defined for Enum type '{typeof(TEnum).Name}'.",
			nameof(value));
	}

	[DoesNotReturn]
	private static void throwNotDefined<TEnum, TUnderlying>(TUnderlying underlying)
	{
		throw new ArgumentException(
			$"The value of argument '{nameof(underlying)}' ({underlying}) is not defined for Enum type '{typeof(TEnum).Name}'.",
			nameof(underlying));
	}

	[DoesNotReturn]
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

		return name;
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

		return name;
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
	public static bool TryGetValue<TEnum, TUnderlying>(TEnum value, [NotNullWhen(true)] out TUnderlying? underlying)
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

	#region casting

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TUnderlying">Integral type of the value.</typeparam>
	/// <returns>An instance of the enumeration set to value.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static TEnum Cast<TEnum, TUnderlying>(TUnderlying underlying)
		where TEnum : struct, Enum
		where TUnderlying : struct, INumber<TUnderlying>
	{
		TEnum casted = (TEnum)(object)underlying;
		AssertDefined(casted);
		return casted;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>An instance of the enumeration set to value.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static TEnum Cast<TEnum>(byte underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, byte>(underlying);
		}

		return casted!.Value;
	}

	/// <inheritdoc cref="Cast{TEnum}(byte)"/>
	public static TEnum Cast<TEnum>(sbyte underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, sbyte>(underlying);
		}

		return casted!.Value;
	}

	/// <inheritdoc cref="Cast{TEnum}(byte)"/>
	public static TEnum Cast<TEnum>(ushort underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, ushort>(underlying);
		}

		return casted!.Value;
	}

	/// <inheritdoc cref="Cast{TEnum}(byte)"/>
	public static TEnum Cast<TEnum>(short underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, short>(underlying);
		}

		return casted!.Value;
	}

	/// <inheritdoc cref="Cast{TEnum}(byte)"/>
	public static TEnum Cast<TEnum>(uint underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, uint>(underlying);
		}

		return casted!.Value;
	}

	/// <inheritdoc cref="Cast{TEnum}(byte)"/>
	public static TEnum Cast<TEnum>(int underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, int>(underlying);
		}

		return casted!.Value;
	}

	/// <inheritdoc cref="Cast{TEnum}(byte)"/>
	public static TEnum Cast<TEnum>(ulong underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, ulong>(underlying);
		}

		return casted!.Value;
	}

	/// <inheritdoc cref="Cast{TEnum}(byte)"/>
	public static TEnum Cast<TEnum>(long underlying)
		where TEnum : struct, Enum
	{
		bool success = TryCast(underlying, out TEnum? casted);
		if (!success)
		{
			throwNotDefined<TEnum, long>(underlying);
		}

		return casted!.Value;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(byte underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(sbyte underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(ushort underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(short underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(uint underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(int underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(ulong underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	/// <summary>
	/// Converts the specified integral value to an enumeration member.
	/// </summary>
	/// <param name="underlying">The value of a particular enumerated constant.</param>
	/// <param name="casted">An instance of the enumeration set to value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns><c>true</c> if <paramref name="underlying"/> was converted successfully; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException"><paramref name="underlying"/> is not defined in <typeparamref name="TEnum"/>.</exception>
	public static bool TryCast<TEnum>(long underlying, [NotNullWhen(true)] out TEnum? casted)
		where TEnum : struct, Enum
	{
		casted = null;
		bool success = IsDefined<TEnum>(underlying);
		if (success)
		{
			casted = (TEnum)Enum.ToObject(typeof(TEnum), underlying);
		}

		return success;
	}

	#endregion

	#region parsing

	/// <summary>
	/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
	/// The return value indicates whether the conversion succeeded.
	/// </summary>
	/// <param name="value">The case-sensitive string representation of the enumeration name or underlying value to convert.</param>
	/// <param name="parsed">When this method returns, contains an object of type <typeparamref name="TEnum"/> whose value is represented by value if the parse operation succeeds.
	/// If the parse operation fails, contains <c>null</c>.
	/// This parameter is passed uninitialized.</param>
	/// <typeparam name="TEnum">The enumeration type to which to convert value.</typeparam>
	/// <returns><c>true</c> if the value parameter was converted successfully; otherwise, <c>false</c>.</returns>
	public static bool TryParse<TEnum>(string value, [NotNullWhen(true)] out TEnum? parsed) where TEnum : struct, Enum
	{
		parsed = null;
		bool success = Enum.TryParse(value, out TEnum p) && IsDefined(p);
		if (success)
		{
			parsed = p;
		}

		return success;
	}

	/// <summary>
	/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
	/// A parameter specifies whether the operation is case-sensitive.
	/// The return value indicates whether the conversion succeeded.
	/// </summary>
	/// <param name="value">The case-sensitive string representation of the enumeration name or underlying value to convert.</param>
	/// <param name="ignoreCase"> true to ignore case; false to consider case.</param>
	/// <param name="parsed">When this method returns, contains an object of type <typeparamref name="TEnum"/> whose value is represented by value if the parse operation succeeds.
	/// If the parse operation fails, contains <c>null</c>.
	/// This parameter is passed uninitialized.</param>
	/// <typeparam name="TEnum">The enumeration type to which to convert value.</typeparam>
	/// <returns><c>true</c> if the value parameter was converted successfully; otherwise, <c>false</c>.</returns>
	public static bool TryParse<TEnum>(string value, bool ignoreCase, [NotNullWhen(true)] out TEnum? parsed)
		where TEnum : struct, Enum
	{
		parsed = null;
		bool success = Enum.TryParse(value, ignoreCase, out TEnum p) && IsDefined(p);
		if (success)
		{
			parsed = p;
		}

		return success;
	}

	/// <summary>
	/// Converts the string representation of the name or numeric value of one or more enumerated constants specified by TEnum to an equivalent enumerated object.
	/// A parameter specifies whether the operation is case-insensitive.
	/// </summary>
	/// <param name="value">A string containing the name or value to convert.</param>
	/// <param name="ignoreCase">true to ignore case; false to regard case.</param>
	/// <typeparam name="TEnum">The enumeration type to which to convert value.</typeparam>
	/// <returns>an object of type <typeparamref name="TEnum"/> whose value is represented by value if the parse operation succeeds.</returns>
	/// <exception cref="ArgumentException"><paramref name="value"/> does not exist in <typeparamref name="TEnum"/>.</exception>
	public static TEnum Parse<TEnum>(string value, bool ignoreCase = false) where TEnum : struct, Enum
	{
		bool result = TryParse(value, ignoreCase, out TEnum? parsed);
		if (!result) throwNotDefined<TEnum, string>(value);
		return parsed!.Value;
	}

	#endregion

	#region omit

	/// <summary>
	/// Removes the values specified by <paramref name="valuesToRemove"/> from the collection of <see cref="GetValues"/>
	/// </summary>
	/// <param name="valuesToRemove">An <see cref="IEnumerable{TEnum}"/> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
	public static IEnumerable<TEnum> Omit<TEnum>(IEnumerable<TEnum> valuesToRemove) where TEnum : struct, Enum =>
		GetValues<TEnum>().Except(valuesToRemove);

	/// <summary>
	/// Removes the values specified by <paramref name="valuesToRemove"/> from the collection of <see cref="GetValues"/>
	/// </summary>
	/// <param name="valuesToRemove">A collection whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
	public static IEnumerable<TEnum> Omit<TEnum>(params TEnum[] valuesToRemove) where TEnum : struct, Enum =>
		Omit(valuesToRemove.AsEnumerable());

	#endregion

	#region reflection

	/// <summary>
	/// Searches for the public field with the specified enumeration value.
	/// </summary>
	/// <param name="value">The enumeration value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>An object representing the public field with the specified name.</returns>
	public static FieldInfo GetField<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		AssertDefined(value);
		string name = value.ToString();
		return typeof(TEnum).GetField(name)!;
	}

	/// <summary>
	/// Indicates whether custom attributes of a specified type are applied to a specified enumeration value.
	/// </summary>
	/// <param name="value">The enumeration value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TAttribute">The type of attribute to search for.</typeparam>
	/// <returns><c>true</c> if an attribute of the specified type is applied to value; otherwise, <c>false</c>.</returns>
	public static bool HasAttribute<TEnum, TAttribute>(TEnum value)
		where TEnum : struct, Enum
		where TAttribute : Attribute =>
		GetField(value).IsDefined(typeof(TAttribute));

	/// <summary>
	/// Retrieves a custom attribute of a specified type that is applied to an enumeration value.
	/// </summary>
	/// <param name="value">The enumeration value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <typeparam name="TAttribute">The type of attribute to search for.</typeparam>
	/// <returns>A custom attribute that matches <typeparamref name="TAttribute"/>, or <c>null</c> if no such attribute is found.</returns>
	public static TAttribute? GetAttribute<TEnum, TAttribute>(TEnum value)
		where TEnum : struct, Enum
		where TAttribute : Attribute => GetField(value).GetCustomAttribute<TAttribute>();

	/// <summary>
	/// Retrieves a custom description that is applied to an enumeration value.
	/// </summary>
	/// <param name="value">The enumeration value.</param>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>A custom description, or <c>null</c> if not found.</returns>
	public static string? GetDescription<TEnum>(TEnum value)
		where TEnum : struct, Enum => GetAttribute<TEnum, DescriptionAttribute>(value)?.Description;

	#endregion

	#region flags

	/// <summary>
	/// Indicates that an enumeration can be treated as a bit field; that is, a set of flags.
	/// </summary>
	/// <typeparam name="TFlags">The type of the flags enumeration.</typeparam>
	/// <returns><c>true</c> is the <typeparamref name="TFlags"/> is decorated with <see cref="FlagsAttribute"/>, <c>false</c> otherwise.</returns>
	public static bool IsFlags<TFlags>() where TFlags : struct, Enum =>
		typeof(TFlags).IsDefined(typeof(FlagsAttribute), false);

	/// <summary>
	/// Indicates that an enumeration can be treated as a bit field; that is, a set of flags.
	/// </summary>
	/// <typeparam name="TFlags">The type of the flags enumeration.</typeparam>
	/// <exception cref="ArgumentException">If the <typeparamref name="TFlags"/> is not decorated with <see cref="FlagsAttribute"/>.</exception>
	public static void AssertFlags<TFlags>() where TFlags : struct, Enum
	{
		if (!IsFlags<TFlags>())
		{
			throw new ArgumentException(
				$"Enum '{typeof(TFlags).Name}' must have {nameof(FlagsAttribute)} applied to it.", nameof(TFlags));
		}
	}

	private static class Flags<TFlags> where TFlags : struct, Enum
	{
		internal static readonly Func<TFlags, TFlags, TFlags> BitwiseOr, BitwiseAnd;
		internal static readonly Func<TFlags, TFlags> Not;

		static Flags()
		{
			Type underlying = GetUnderlyingType<TFlags>();
			Type tFlags = typeof(TFlags);
			ParameterExpression param1 = Expression.Parameter(tFlags, "x");
			ParameterExpression param2 = Expression.Parameter(tFlags, "y");
			Expression convertedParam1 = Expression.Convert(param1, underlying);
			Expression convertedParam2 = Expression.Convert(param2, underlying);

			BitwiseOr = Expression.Lambda<Func<TFlags, TFlags, TFlags>>(
					Expression.Convert(Expression.Or(convertedParam1, convertedParam2), tFlags), param1, param2)
				.Compile();
			BitwiseAnd = Expression.Lambda<Func<TFlags, TFlags, TFlags>>(
					Expression.Convert(Expression.And(convertedParam1, convertedParam2), tFlags), param1, param2)
				.Compile();
			Not = Expression.Lambda<Func<TFlags, TFlags>>(
					Expression.Convert(Expression.Not(convertedParam1), tFlags), param1)
				.Compile();
		}
	}

	/// <summary>
	/// Sets the specified flag on the provided flags value.
	/// </summary>
	/// <param name="flags">The flags value.</param>
	/// <param name="flagToSet">The flag to set.</param>
	/// <typeparam name="TFlags">The enum type decorated with <see cref="FlagsAttribute"/>.</typeparam>
	/// <returns>The resulting flags value after setting the specified flag.</returns>
	public static TFlags SetFlag<TFlags>(this TFlags flags, TFlags flagToSet) where TFlags : struct, Enum
	{
		AssertFlags<TFlags>();
		return Flags<TFlags>.BitwiseOr(flags, flagToSet);
	}

	/// <summary>
	/// Removes the specified flag from the provided flags value.
	/// </summary>
	/// <param name="flags">The flags value.</param>
	/// <param name="flagToSet">The flag to remove.</param>
	/// <typeparam name="TFlags">The enum type decorated with <see cref="FlagsAttribute"/>.</typeparam>
	/// <returns>The resulting flags value after removing the specified flag.</returns>
	public static TFlags UnsetFlag<TFlags>(this TFlags flags, TFlags flagToSet) where TFlags : struct, Enum
	{
		AssertFlags<TFlags>();
		return Flags<TFlags>.BitwiseAnd(flags, Flags<TFlags>.Not(flagToSet));
	}

	/// <summary>
	/// Toggles the specified flag on the provided flags value.
	/// </summary>
	/// <param name="flags">The flags value.</param>
	/// <param name="flagToToggle">The flag to toggle.</param>
	/// <typeparam name="TFlags">The enum type decorated with <see cref="FlagsAttribute"/>.</typeparam>
	/// <returns>The resulting flags value after toggling the specified flag.</returns>
	public static TFlags ToggleFlag<TFlags>(this TFlags flags, TFlags flagToToggle) where TFlags : struct, Enum
	{
		AssertFlags<TFlags>();
		return flags.HasFlag(flagToToggle) ? UnsetFlag(flags, flagToToggle) : SetFlag(flags, flagToToggle);
	}

	/// <summary>
	/// Returns the individual flag values that are set on the provided flags value.
	/// </summary>
	/// <param name="flag">The flags value to inspect.</param>
	/// <param name="ignoreZeroValue">If <c>true</c>, the zero/default value (if present among enum values) will be omitted from the returned sequence.</param>
	/// <typeparam name="TFlags">The enum type decorated with <see cref="FlagsAttribute"/>.</typeparam>
	/// <returns>An <see cref="IEnumerable{TFlags}"/> containing each flag value that is set on <paramref name="flag"/>.</returns>
	/// <exception cref="ArgumentException">Thrown if <typeparamref name="TFlags"/> is not a flags enum.</exception>
	public static IEnumerable<TFlags> GetFlags<TFlags>(this TFlags flag, bool ignoreZeroValue = false)
		where TFlags : struct, Enum
	{
		AssertFlags<TFlags>();
		// flags always have the default as the option with value zero (regardless of whether is defined or not)
		// so ignoring the zero value means leaving out the default value altogether from the list of values
		return GetValues<TFlags>()
			// boxing, again
			.Where(t => !(ignoreZeroValue && Equals(t, default(TFlags))))
			.Where(t => flag.HasFlag(t));
	}

	#endregion

	#region translation

	/// <summary>
	/// Translates the specified <paramref name="source"/> enum value to the target enum type by matching names.
	/// </summary>
	/// <param name="source">The source enum value to translate.</param>
	/// <param name="ignoreCase">Whether the translation should ignore case when matching names.</param>
	/// <typeparam name="TSource">The source enum type.</typeparam>
	/// <typeparam name="TTarget">The target enum type.</typeparam>
	/// <returns>The translated <typeparamref name="TTarget"/> value.</returns>
	/// <exception cref="ArgumentException">If the translated name does not exist in <typeparamref name="TTarget"/>.</exception>
	public static TTarget Translate<TSource, TTarget>(TSource source, bool ignoreCase = false)
		where TSource : struct, Enum
		where TTarget : struct, Enum =>
		Parse<TTarget>(source.ToString(), ignoreCase);

	/// <summary>
	/// Attempts to translate the specified <paramref name="source"/> enum value to the target enum type.
	/// </summary>
	/// <param name="source">The source enum value to translate.</param>
	/// <param name="translated">When this method returns, contains the translated value if the translation succeeded; otherwise, <c>null</c>.</param>
	/// <param name="ignoreCase">Whether the translation should ignore case when matching names.</param>
	/// <typeparam name="TSource">The source enum type.</typeparam>
	/// <typeparam name="TTarget">The target enum type.</typeparam>
	/// <returns><c>true</c> if translation succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryTranslate<TSource, TTarget>(TSource source, out TTarget? translated, bool ignoreCase = false)
		where TSource : struct, Enum
		where TTarget : struct, Enum =>
		TryParse(source.ToString(), ignoreCase, out translated);

	/// <summary>
	/// Attempts to translate the specified <paramref name="source"/> enum value to the target enum type and returns the translated value
	/// or the provided <paramref name="defaultValue"/> when translation fails.
	/// </summary>
	/// <param name="source">The source enum value to translate.</param>
	/// <param name="defaultValue">The default value to return if translation fails.</param>
	/// <param name="ignoreCase">Whether the translation should ignore case when matching names.</param>
	/// <typeparam name="TSource">The source enum type.</typeparam>
	/// <typeparam name="TTarget">The target enum type.</typeparam>
	/// <returns>The translated <typeparamref name="TTarget"/> value or <paramref name="defaultValue"/> if translation failed.</returns>
	public static TTarget Translate<TSource, TTarget>(TSource source, TTarget defaultValue, bool ignoreCase = false)
		where TSource : struct, Enum
		where TTarget : struct, Enum
	{
		TryParse(source.ToString(), ignoreCase, out TTarget? translated);
		return translated.GetValueOrDefault(defaultValue);
	}

	#endregion

	#region fast comparison

	[SuppressMessage("Style", "IDE1006:Naming Styles")]
	internal class FastEnumComparer<TEnum> : IEqualityComparer<TEnum>
		where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		public static readonly IEqualityComparer<TEnum> Instance;

		private static readonly Func<TEnum, TEnum, bool> equals;
		private static readonly Func<TEnum, int> getHashCode;

		static FastEnumComparer()
		{
			getHashCode = generateGetHashCode();
			equals = generateEquals();
			Instance = new FastEnumComparer<TEnum>();
		}

		/// <summary>
		/// A private constructor to prevent user instantiation.
		/// </summary>
		private FastEnumComparer() => assertUnderlyingTypeIsSupported();

		// call the generated method
		public bool Equals(TEnum x, TEnum y) => equals(x, y);

		// call the generated method
		public int GetHashCode(TEnum obj) => getHashCode(obj);

		private static void assertUnderlyingTypeIsSupported()
		{
			var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			ICollection<Type> supportedTypes =
			[
				typeof(byte), typeof(sbyte),
				typeof(short), typeof(ushort),
				typeof(int), typeof(uint),
				typeof(long), typeof(ulong)
			];

			if (!supportedTypes.Contains(underlyingType))
			{
				string typeNames = string.Join(", ", supportedTypes.Select(t => t.Name));
				throw new NotSupportedException(
					$"The underlying type of '{typeof(TEnum).Name}' is {underlyingType.Name}. Only enums with underlying type of [{typeNames}] are supported.");
			}
		}

		private static Func<TEnum, TEnum, bool> generateEquals()
		{
			var xParam = Expression.Parameter(typeof(TEnum), "x");
			var yParam = Expression.Parameter(typeof(TEnum), "y");
			var equalExpression = Expression.Equal(xParam, yParam);
			return Expression.Lambda<Func<TEnum, TEnum, bool>>(equalExpression, xParam, yParam).Compile();
		}

		private static Func<TEnum, int> generateGetHashCode()
		{
			var objParam = Expression.Parameter(typeof(TEnum), "obj");
			var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			var convertExpression = Expression.Convert(objParam, underlyingType);
			var getHashCodeMethod = underlyingType.GetTypeInfo().GetMethod("GetHashCode");
			var getHashCodeExpression = Expression.Call(convertExpression, getHashCodeMethod!);
			return Expression.Lambda<Func<TEnum, int>>(getHashCodeExpression, new[] { objParam }).Compile();
		}
	}

	/// <summary>
	/// Returns an equality comparer optimized for the enum type <typeparamref name="TEnum"/>.
	/// </summary>
	/// <typeparam name="TEnum">The enum type to compare.</typeparam>
	/// <returns>An <see cref="IEqualityComparer{TEnum}"/> that efficiently compares values of <typeparamref name="TEnum"/>.</returns>
	public static IEqualityComparer<TEnum> GetComparer<TEnum>() where TEnum : struct, Enum =>
		FastEnumComparer<TEnum>.Instance;

	#endregion
}