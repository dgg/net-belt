using System.Numerics;

namespace Net.Belt.Extensions.Numerics;

/// <summary>
/// Provides extension methods for numeric types.
/// </summary>
public static class NumericsExtensions
{
	/// <summary>
	/// Determines whether the specified value is a numeric type.
	/// </summary>
	/// <param name="maybeNumeric">The value to check.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns><c>true</c> if the value is a numeric type; otherwise, <c>false</c>.</returns>
	public static bool IsNumeric<T>(this T maybeNumeric) =>
		closesInterface<T>(typeof(INumber<>));
	
	/// <summary>
	/// Determines whether the specified value is a boxed numeric type.
	/// </summary>
	/// <param name="maybeNumeric">The boxed value to check.</param>
	/// <returns><c>true</c> if the boxed value is a numeric type; otherwise, <c>false</c>.</returns>
	public static bool IsBoxedNumeric(this object maybeNumeric) =>
		closesInterface(maybeNumeric.GetType(), typeof(INumber<>));
	
	/// <summary>
	/// Determines whether the specified value is an integral type.
	/// </summary>
	/// <param name="maybeIntegral">The value to check.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns><c>true</c> if the value is an integral type; otherwise, <c>false</c>.</returns>
	public static bool IsIntegral<T>(this T maybeIntegral) =>
		closesInterface<T>(typeof(IBinaryInteger<>));
	
	/// <summary>
	/// Determines whether the specified boxed value is an integral type.
	/// </summary>
	/// <param name="maybeIntegral">The boxed value to check.</param>
	/// <returns><c>true</c> if the value is an integral type; otherwise, <c>false</c>.</returns>
	public static bool IsBoxedIntegral(this object maybeIntegral) =>
		closesInterface(maybeIntegral.GetType(), typeof(IBinaryInteger<>));

	/// <summary>
	/// Determines whether the specified value is a real number type.
	/// </summary>
	/// <param name="maybeReal">The value to check.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns><c>true</c> if the value is a real number type; otherwise, <c>false</c>.</returns>
	public static bool IsReal<T>(this T maybeReal) =>
		closesInterface<T>(typeof(IFloatingPoint<>));
	
	/// <summary>
	/// Determines whether the specified boxed value is a real number type.
	/// </summary>
	/// <param name="maybeIntegral">The boxed value to check.</param>
	/// <returns><c>true</c> if the value is a real number type; otherwise, <c>false</c>.</returns>
	public static bool IsBoxedReal(this object maybeIntegral) =>
		closesInterface(maybeIntegral.GetType(), typeof(IFloatingPoint<>));

	/// <summary>
	/// Determines whether the specified value is a signed number type.
	/// </summary>
	/// <param name="maybeSigned">The value to check.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns><c>true</c> if the value is a signed number type; otherwise, <c>false</c>.</returns>
	public static bool IsSigned<T>(this T maybeSigned) =>
		closesInterface<T>(typeof(ISignedNumber<>));
	
	/// <summary>
	/// Determines whether the specified boxed value is a signed number type.
	/// </summary>
	/// <param name="maybeSigned">The boxed value to check.</param>
	/// <returns><c>true</c> if the boxed value is a signed number type; otherwise, <c>false</c>.</returns>
	public static bool IsBoxedSigned(this object maybeSigned) =>
		closesInterface(maybeSigned.GetType(), typeof(ISignedNumber<>));

	/// <summary>
	/// Determines whether the specified value is an unsigned number type.
	/// </summary>
	/// <param name="maybeUnsigned">The value to check.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns><c>true</c> if the value is an unsigned number type; otherwise, <c>false</c>.</returns>
	public static bool IsUnsigned<T>(this T maybeUnsigned) =>
		closesInterface<T>(typeof(IUnsignedNumber<>));
	
	/// <summary>
	/// Determines whether the specified boxed value is an unsigned number type.
	/// </summary>
	/// <param name="maybeUnsigned">The boxed value to check.</param>
	/// <returns><c>true</c> if the boxed value is an unsigned number type; otherwise, <c>false</c>.</returns>
	public static bool IsBoxedUnsigned(this object maybeUnsigned) =>
		closesInterface(maybeUnsigned.GetType(),typeof(IUnsignedNumber<>));

	private static bool closesInterface<T>(Type openGenerics) =>
		closesInterface(typeof(T), openGenerics);

	private static bool closesInterface(Type type, Type openGenerics)
	{
		return type.GetInterfaces()
			.Where(i => i.IsGenericType)
			.Any(i => i.GetGenericTypeDefinition() == openGenerics);
	}
}