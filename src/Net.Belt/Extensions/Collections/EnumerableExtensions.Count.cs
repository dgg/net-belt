namespace Net.Belt.Extensions.Collections;

/// <summary>
/// Provides extension methods for checking the existence and count of elements in an <see cref="IEnumerable{T}"/>.
/// </summary>
/// <param name="source">The source enumerable.</param>
/// <typeparam name="T">The type of elements in the enumerable.</typeparam>
public readonly struct HasExtensions<T>(IEnumerable<T> source)
{
    /// <summary>
    /// Determines whether a sequence contains exactly one element.
    /// </summary>
    /// <returns><see langword="true"/> if the source sequence contains exactly one element; otherwise, <see langword="false"/>.</returns>
    public bool One()
    {
        using IEnumerator<T> enumerator = source.GetEnumerator();
        bool hasOne = enumerator.MoveNext() && !enumerator.MoveNext();
        return hasOne;
    }

    /// <summary>
    /// Determines whether a sequence contains at least a specified number of elements.
    /// </summary>
    /// <param name="count">The minimum number of elements the sequence must contain.</param>
    /// <returns><see langword="true"/> if the source sequence contains at least <paramref name="count"/> elements; otherwise, <see langword="false"/>.</returns>
    public bool AtLeast(uint count)
    {
        uint i = 0;
        using IEnumerator<T> enumerator = source.GetEnumerator();
        while (i <= count && enumerator.MoveNext())
        {
            i++;
        }

        bool atLeast = i >= count;
        return atLeast;
    }
}