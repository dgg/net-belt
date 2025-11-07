namespace Net.Belt.Comparisons;

/// <summary>
/// Provides hashing utilities to generate hash codes for objects and values according to various strategies.
/// </summary>
public static class Hasher
{

	/// <summary>
	/// Generates a hash code for eight values using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <typeparam name="T6">The type of the sixth value.</typeparam>
	/// <typeparam name="T7">The type of the seventh value.</typeparam>
	/// <typeparam name="T8">The type of the eighth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <param name="t6">The sixth value.</param>
	/// <param name="t7">The seventh value.</param>
	/// <param name="t8">The eighth value.</param>
	/// <returns>The combined hash code.</returns>
	public static int Default<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) =>
		HashCode.Combine(t1, t2, t3, t4, t5, t6, t7, t8);

	/// <summary>
	/// Generates a hash code for seven values using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <typeparam name="T6">The type of the sixth value.</typeparam>
	/// <typeparam name="T7">The type of the seventh value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <param name="t6">The sixth value.</param>
	/// <param name="t7">The seventh value.</param>
	/// <returns>The combined hash code.</returns>
	public static int Default<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) =>
		HashCode.Combine(t1, t2, t3, t4, t5, t6, t7);

	/// <summary>
	/// Generates a hash code for six values using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <typeparam name="T6">The type of the sixth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <param name="t6">The sixth value.</param>
	/// <returns>The combined hash code.</returns>
	public static int Default<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) =>
		HashCode.Combine(t1, t2, t3, t4, t5, t6);

	/// <summary>
	/// Generates a hash code for five values using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <returns>The combined hash code.</returns>
	public static int Default<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) =>
		HashCode.Combine(t1, t2, t3, t4, t5);

	/// <summary>
	/// Generates a hash code for four values using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <returns>The combined hash code.</returns>
	public static int Default<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4) => HashCode.Combine(t1, t2, t3, t4);

	/// <summary>
	/// Generates a hash code for three values using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <returns>The combined hash code.</returns>
	public static int Default<T1, T2, T3>(T1 t1, T2 t2, T3 t3) => HashCode.Combine(t1, t2, t3);

	/// <summary>
	/// Generates a hash code for two values using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <returns>The combined hash code.</returns>
	public static int Default<T1, T2>(T1 t1, T2 t2) => HashCode.Combine(t1, t2);

	/// <summary>
	/// Generates a hash code for a single value using the default hash code combiner.
	/// </summary>
	/// <typeparam name="T1">The type of the value.</typeparam>
	/// <param name="t1">The value to hash.</param>
	/// <returns>The hash code for the value.</returns>
	public static int Default<T1>(T1 t1) => HashCode.Combine(t1);

	/// <summary>
	/// Generates a hash code with a value of zero for the given object or value.
	/// </summary>
	/// <typeparam name="T">The type of the object or value.</typeparam>
	/// <param name="_">The object or value for which to generate the hash code. Ignored and does not influence the result.</param>
	/// <returns>An integer representing a hash code of always zero.</returns>
	public static int Zero<T>(T _) => 0;

	/// <summary>
	/// Generates a canonical hash code for eight values using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <typeparam name="T6">The type of the sixth value.</typeparam>
	/// <typeparam name="T7">The type of the seventh value.</typeparam>
	/// <typeparam name="T8">The type of the eighth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <param name="t6">The sixth value.</param>
	/// <param name="t7">The seventh value.</param>
	/// <param name="t8">The eighth value.</param>
	/// <returns>The canonical combined hash code.</returns>
	public static int Canonical<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			hashCode = (hashCode * 397) ^ (t2?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t3?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t4?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t5?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t6?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t7?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t8?.GetHashCode() ?? 0);
			return hashCode;
		}
	}

	/// <summary>
	/// Generates a canonical hash code for seven values using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <typeparam name="T6">The type of the sixth value.</typeparam>
	/// <typeparam name="T7">The type of the seventh value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <param name="t6">The sixth value.</param>
	/// <param name="t7">The seventh value.</param>
	/// <returns>The canonical combined hash code.</returns>
	public static int Canonical<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			hashCode = (hashCode * 397) ^ (t2?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t3?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t4?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t5?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t6?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t7?.GetHashCode() ?? 0);
			return hashCode;
		}
	}

	/// <summary>
	/// Generates a canonical hash code for six values using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <typeparam name="T6">The type of the sixth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <param name="t6">The sixth value.</param>
	/// <returns>The canonical combined hash code.</returns>
	public static int Canonical<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			hashCode = (hashCode * 397) ^ (t2?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t3?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t4?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t5?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t6?.GetHashCode() ?? 0);
			return hashCode;
		}
	}

	/// <summary>
	/// Generates a canonical hash code for five values using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <typeparam name="T5">The type of the fifth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <param name="t5">The fifth value.</param>
	/// <returns>The canonical combined hash code.</returns>
	public static int Canonical<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			hashCode = (hashCode * 397) ^ (t2?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t3?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t4?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t5?.GetHashCode() ?? 0);
			return hashCode;
		}
	}

	/// <summary>
	/// Generates a canonical hash code for four values using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <typeparam name="T4">The type of the fourth value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <param name="t4">The fourth value.</param>
	/// <returns>The canonical combined hash code.</returns>
	public static int Canonical<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			hashCode = (hashCode * 397) ^ (t2?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t3?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t4?.GetHashCode() ?? 0);
			return hashCode;
		}
	}

	/// <summary>
	/// Generates a canonical hash code for three values using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <typeparam name="T3">The type of the third value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <param name="t3">The third value.</param>
	/// <returns>The canonical combined hash code.</returns>
	public static int Canonical<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			hashCode = (hashCode * 397) ^ (t2?.GetHashCode() ?? 0);
			hashCode = (hashCode * 397) ^ (t3?.GetHashCode() ?? 0);
			return hashCode;
		}
	}

	/// <summary>
	/// Generates a canonical hash code for two values using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the first value.</typeparam>
	/// <typeparam name="T2">The type of the second value.</typeparam>
	/// <param name="t1">The first value.</param>
	/// <param name="t2">The second value.</param>
	/// <returns>The canonical combined hash code.</returns>
	public static int Canonical<T1, T2>(T1 t1, T2 t2)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			hashCode = (hashCode * 397) ^ (t2?.GetHashCode() ?? 0);
			return hashCode;
		}
	}

	/// <summary>
	/// Generates a canonical hash code for a single value using a custom algorithm.
	/// </summary>
	/// <typeparam name="T1">The type of the value.</typeparam>
	/// <param name="t1">The value to hash.</param>
	/// <returns>The canonical hash code for the value.</returns>
	public static int Canonical<T1>(T1 t1)
	{
		unchecked
		{
			var hashCode = t1?.GetHashCode() ?? 0;
			return hashCode;
		}
	}

	/// <summary>
	/// Creates a new instance of <see cref="HashCode"/> for a given value and adds the value's hash to the instance.
	/// </summary>
	/// <typeparam name="T">The type of the object or value to be hashed.</typeparam>
	/// <param name="value">The object or value for which to generate and store the hash code.</param>
	/// <param name="comparer">An optional equality comparer to compute the hash code for the value. If null, the default comparer is used.</param>
	/// <returns>A <see cref="HashCode"/> instance initialized with the hash code of the provided value.</returns>
	public static HashCode Fluent<T>(T value, IEqualityComparer<T>? comparer = null)
	{
		var hashCode = new HashCode();
		hashCode.Add(value, comparer);
		return hashCode;
	}

	/// <summary>
	/// Adds a value to the current hash code computation and returns the updated HashCode instance.
	/// </summary>
	/// <typeparam name="T">The type of the value to be added to the hash code.</typeparam>
	/// <param name="hashCode">The current HashCode instance that is being updated.</param>
	/// <param name="value">The value to be added to the hash code. Can be null.</param>
	/// <param name="comparer">An optional equality comparer to use for generating the hash code of the value. If null, the default comparer is used.</param>
	/// <returns>The updated HashCode instance that incorporates the value.</returns>
	public static HashCode Hashing<T>(this HashCode hashCode, T value, IEqualityComparer<T>? comparer = null)
	{
		hashCode.Add(value, comparer);
		return hashCode;
	}
}