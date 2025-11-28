using System.Collections;

namespace Net.Belt.Collections;

/// <summary>
/// Provides a way to enumerate a sequence with additional information about each element's position:
/// its index, whether it's the first element, and whether it's the last element.
/// </summary>
/// <param name="enumerable">The enumerable sequence to wrap.</param>
/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
public class SmartEnumerable<T>(IEnumerable<T> enumerable) : IEnumerable<SmartEntry<T>>
{
	/// <inheritdoc />
	public IEnumerator<SmartEntry<T>> GetEnumerator()
	{
		using IEnumerator<T> enumerator = enumerable.GetEnumerator();
		if (!enumerator.MoveNext()) { yield break; }
		bool isFirst = true;
		bool isLast = false;
		uint index = 0;
		while (!isLast)
		{
			T current = enumerator.Current;
			isLast = !enumerator.MoveNext();
			yield return new SmartEntry<T>(index++, current, isFirst, isLast);
			isFirst = false;
		}
	}
	
	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}