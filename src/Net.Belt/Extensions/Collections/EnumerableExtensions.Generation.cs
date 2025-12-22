namespace Net.Belt.Extensions.Collections;

/// <summary>
/// 
/// </summary>
/// <param name="source"></param>
/// <typeparam name="T"></typeparam>
public readonly struct GenerationExtensions<T>(IEnumerable<T> source)
{
	/// <summary>
	/// Returns a new enumerable that skips a specified number of elements after each yielded element.
	/// </summary>
	/// <param name="every">The number of elements to skip after each yielded element. Must be greater than or equal to 0.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence, skipping <paramref name="every"/> elements after each one.</returns>
	/// <remarks>
	/// If <paramref name="every"/> is 0, all elements are returned.
	/// If the source sequence runs out of elements while skipping, the enumeration stops.
	/// </remarks>
	public IEnumerable<T> Skipping(uint every)
	{
		using IEnumerator<T> enumerator = source.GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current;

			for (uint i = every; i > 0; i--)
				if (!enumerator.MoveNext())
					break;
		}
	}

	/// <summary>
	/// Interlaces elements from the source enumerable with elements from another enumerable.
	/// </summary>
	/// <param name="second">The second enumerable to interlace with the source enumerable.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> that contains elements from both enumerables, alternating between them.</returns>
	public IEnumerable<T> Interlace(IEnumerable<T> second)
	{
		using IEnumerator<T> e1 = source.GetEnumerator();
		using IEnumerator<T> e2 = second.GetEnumerator();
		while (e1.MoveNext() && e2.MoveNext())
		{
			yield return e1.Current;
			yield return e2.Current;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="batchSize"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public IEnumerable<IEnumerable<T>> InBatchesOf(ushort batchSize)
	{
		if (batchSize == 0)
		{
			throw new ArgumentOutOfRangeException(nameof(batchSize), batchSize, "Batch size cannot be zero.");
		}

		T[]? bucket = null;
		var count = 0;

		foreach (var item in source)
		{
			bucket ??= new T[batchSize];

			bucket[count++] = item;

			if (count != batchSize) continue;

			// performance hack for array elements
			yield return bucket.Select(x => x);

			bucket = null;
			count = 0;
		}

		// Return the last bucket with all remaining elements
		if (bucket != null && count > 0)
		{
			yield return bucket.Take(count);
		}
	}
}