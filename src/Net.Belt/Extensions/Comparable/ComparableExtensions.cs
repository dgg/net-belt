namespace Net.Belt.Extensions.Comparable;

/// <summary>
/// Provides extension methods for comparing values that implement <see cref="IComparable{T}"/>
/// or <see cref="IComparable"/>. These helper methods simplify common comparison operations
/// such as equality, inequality, less-than, greater-than and range checks.
/// </summary>
public static class ComparableExtensions
{
	#region IComparable<>

	/// <summary>
	/// Determines whether two values of type <typeparamref name="T"/> are equal using
	/// the default <see cref="Comparer{T}"/> for <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are equal; otherwise <c>false</c>.</returns>
	/// <remarks>
	/// Uses <see cref="Comparer{T}.Default"/> to perform the comparison.
	/// </remarks>
	public static bool IsEqualTo<T>(this T first, T second) where T : IComparable<T> => IsEqualTo(first, second, Comparer<T>.Default);

	/// <summary>
	/// Determines whether two values of type <typeparamref name="T"/> are equal using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <param name="comparer">The comparer used to compare the values.</param>
	/// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are equal according to <paramref name="comparer"/>; otherwise <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
	public static bool IsEqualTo<T>(this T first, T second, IComparer<T> comparer) => comparer.Compare(first, second) == 0;

	/// <summary>
	/// Determines whether two values of type <typeparamref name="T"/> are different (not equal)
	/// using the default comparer for <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are not equal; otherwise <c>false</c>.</returns>
	/// <remarks>
	/// This is the logical negation of <see cref="IsEqualTo{T}(T,T)"/>.
	/// </remarks>
	public static bool IsDifferentFrom<T>(this T first, T second) where T : IComparable<T> => !IsEqualTo(first, second);

	/// <summary>
	/// Determines whether two values of type <typeparamref name="T"/> are different (not equal)
	/// using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <param name="comparer">The comparer used to compare the values.</param>
	/// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are not equal according to <paramref name="comparer"/>; otherwise <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
	public static bool IsDifferentFrom<T>(this T first, T second, IComparer<T> comparer) => !IsEqualTo(first, second, comparer);

	/// <summary>
	/// Determines whether <paramref name="first"/> is less than or equal to <paramref name="second"/>
	/// using the default comparer for <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is less than or equal to <paramref name="second"/>; otherwise <c>false</c>.</returns>
	public static bool IsAtMost<T>(this T first, T second) where T : IComparable<T> => IsAtMost(first, second, Comparer<T>.Default);

	/// <summary>
	/// Determines whether <paramref name="first"/> is less than or equal to <paramref name="second"/>
	/// using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <param name="comparer">The comparer used to compare the values.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is less than or equal to <paramref name="second"/> according to <paramref name="comparer"/>; otherwise <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
	public static bool IsAtMost<T>(this T first, T second, IComparer<T> comparer) => comparer.Compare(first, second) <= 0;

	/// <summary>
	/// Determines whether <paramref name="first"/> is greater than or equal to <paramref name="second"/>
	/// using the default comparer for <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is greater than or equal to <paramref name="second"/>; otherwise <c>false</c>.</returns>
	public static bool IsAtLeast<T>(this T first, T second) where T : IComparable<T> => IsAtLeast(first, second, Comparer<T>.Default);

	/// <summary>
	/// Determines whether <paramref name="first"/> is greater than or equal to <paramref name="second"/>
	/// using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <param name="comparer">The comparer used to compare the values.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is greater than or equal to <paramref name="second"/> according to <paramref name="comparer"/>; otherwise <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
	public static bool IsAtLeast<T>(this T first, T second, IComparer<T> comparer) => comparer.Compare(first, second) >= 0;

	/// <summary>
	/// Determines whether <paramref name="first"/> is strictly less than <paramref name="second"/>
	/// using the default comparer for <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is less than <paramref name="second"/>; otherwise <c>false</c>.</returns>
	public static bool IsLessThan<T>(this T first, T second) where T : IComparable<T> => IsLessThan(first, second, Comparer<T>.Default);

	/// <summary>
	/// Determines whether <paramref name="first"/> is strictly less than <paramref name="second"/>
	/// using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <param name="comparer">The comparer used to compare the values.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is less than <paramref name="second"/> according to <paramref name="comparer"/>; otherwise <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
	public static bool IsLessThan<T>(this T first, T second, IComparer<T> comparer) => comparer.Compare(first, second) < 0;

	/// <summary>
	/// Determines whether <paramref name="first"/> is strictly greater than <paramref name="second"/>
	/// using the default comparer for <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is greater than <paramref name="second"/>; otherwise <c>false</c>.</returns>
	public static bool IsMoreThan<T>(this T first, T second) where T : IComparable<T> => IsMoreThan(first, second, Comparer<T>.Default);

	/// <summary>
	/// Determines whether <paramref name="first"/> is strictly greater than <paramref name="second"/>
	/// using the specified comparer.
	/// </summary>
	/// <typeparam name="T">The type of the values to compare.</typeparam>
	/// <param name="first">The first value to compare.</param>
	/// <param name="second">The second value to compare.</param>
	/// <param name="comparer">The comparer used to compare the values.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is greater than <paramref name="second"/> according to <paramref name="comparer"/>; otherwise <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="comparer"/> is <c>null</c>.</exception>
	public static bool IsMoreThan<T>(this T first, T second, IComparer<T> comparer) => comparer.Compare(first, second) > 0;

	#endregion

	#region IComparable

	/// <summary>
	/// Determines whether an <see cref="IComparable"/> instance is equal to another object using
	/// the default object comparer.
	/// </summary>
	/// <param name="first">The <see cref="IComparable"/> instance on which the method is called.</param>
	/// <param name="second">The object to compare to.</param>
	/// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are considered equal; otherwise <c>false</c>.</returns>
	/// <remarks>
	/// Comparison is performed via <see cref="Comparer{T}.Default"/> for <see cref="object"/> or by the object's own <see cref="IComparable.CompareTo"/> implementation,
	/// depending on runtime types. If types are not compatible for comparison, the underlying comparison may throw.
	/// </remarks>


	public static bool IsEqualTo(this IComparable first, object second) => Comparer<object>.Default.Compare(first, second) == 0;

	/// <summary>
	/// Determines whether an <see cref="IComparable"/> instance is different (not equal) to another object.
	/// </summary>
	/// <param name="first">The <see cref="IComparable"/> instance on which the method is called.</param>
	/// <param name="second">The object to compare to.</param>
	/// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are not equal; otherwise <c>false</c>.</returns>


	public static bool IsDifferentFrom(this IComparable first, object second) => !IsEqualTo(first, second);

	/// <summary>
	/// Determines whether an <see cref="IComparable"/> instance is less than or equal to another object
	/// using <see cref="IComparable.CompareTo"/>.
	/// </summary>
	/// <param name="first">The <see cref="IComparable"/> instance on which the method is called.</param>
	/// <param name="second">The object to compare to.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is less than or equal to <paramref name="second"/>; otherwise <c>false</c>.</returns>
	/// <remarks>
	/// The call forwards to <see cref="IComparable.CompareTo(object)"/> and therefore may throw an exception
	/// if the types are not compatible or if <paramref name="first"/> is <c>null</c>.
	/// </remarks>
	public static bool IsAtMost(this IComparable first, object second) => first.CompareTo(second) <= 0;

	/// <summary>
	/// Determines whether an <see cref="IComparable"/> instance is greater than or equal to another object
	/// using <see cref="IComparable.CompareTo"/>.
	/// </summary>
	/// <param name="first">The <see cref="IComparable"/> instance on which the method is called.</param>
	/// <param name="second">The object to compare to.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is greater than or equal to <paramref name="second"/>; otherwise <c>false</c>.</returns>
	public static bool IsAtLeast(this IComparable first, object second) => first.CompareTo(second) >= 0;

	/// <summary>
	/// Determines whether an <see cref="IComparable"/> instance is strictly less than another object
	/// using <see cref="IComparable.CompareTo"/>.
	/// </summary>
	/// <param name="first">The <see cref="IComparable"/> instance on which the method is called.</param>
	/// <param name="second">The object to compare to.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is less than <paramref name="second"/>; otherwise <c>false</c>.</returns>
	public static bool IsLessThan(this IComparable first, object second) => first.CompareTo(second) < 0;

	/// <summary>
	/// Determines whether an <see cref="IComparable"/> instance is strictly greater than another object
	/// using <see cref="IComparable.CompareTo"/>.
	/// </summary>
	/// <param name="first">The <see cref="IComparable"/> instance on which the method is called.</param>
	/// <param name="second">The object to compare to.</param>
	/// <returns><c>true</c> if <paramref name="first"/> is greater than <paramref name="second"/>; otherwise <c>false</c>.</returns>
	public static bool IsMoreThan(this IComparable first, object second) => first.CompareTo(second) > 0;

	#endregion

	// public static IComparer<T> Reverse<T>(this IComparer<T> comparer) => new ReversedComparer<T>(comparer);
}