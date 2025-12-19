namespace Net.Belt.Extensions.Collections;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    #region nullability
    
    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that handle nullability.
    /// </summary>
    /// <param name="source">The source enumerable.</param>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    extension<T>(IEnumerable<T>? source)
    {
        /// <summary>
        /// Returns an empty enumerable if the source is <see langword="null"/>; otherwise, returns the source enumerable.
        /// </summary>
        public IEnumerable<T> EmptyIfNull => source ?? [];
    }
    
    /// <summary>
    /// Filters out <see langword="null"/> values from an enumerable of nullable elements.
    /// </summary>
    /// <param name="source">The source enumerable of nullable elements.</param>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <returns>An enumerable containing only the non-<see langword="null"/> elements.</returns>
    public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T?> source) => source.Where(s => s is not null)!;

    #endregion
    
    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that operate on non-nullable enumerables.
    /// </summary>
    /// <param name="source">The source enumerable.</param>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    extension<T>(IEnumerable<T> source)
    {
        /// <summary>
        /// Gets an object that provides extension methods for checking the existence and count of elements in the enumerable.
        /// </summary>
        public HasExtensions<T> Has => new(source);
        
        #region iteration

        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element.</param>
        public void ForEach(Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
        
        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>, providing the element and its zero-based index.
        /// </summary>
        /// <param name="action">The <see cref="Action{T1,T2}"/> delegate to perform on each element, where T1 is the element and T2 is its index.</param>
        public void For(Action<T, uint> action)
        {
            uint i = 0;
            foreach (var item in source)
            {
                action(item, i++);
            }
        }
        
        /// <summary>
        /// Performs the specified action on elements of the <see cref="IEnumerable{T}"/> at specific zero-based indexes.
        /// </summary>
        /// <param name="action">The <see cref="Action{T1,T2}"/> delegate to perform on the selected elements, where T1 is the element and T2 is its index.</param>
        /// <param name="indexes">A collection of zero-based indexes indicating which elements to apply the action to.</param>
        public void For(Action<T, uint> action, IEnumerable<uint> indexes)
        {
            var set = new HashSet<uint>(indexes);

            uint i = 0;
            foreach (var item in source)
            {
                if (set.Contains(i))
                {
                    action(item, i);
                }
                i++;
            }
        }
        
        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/> and then yields the element.
        /// This allows for side effects during enumeration while still returning the original sequence.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the original elements after the action has been performed.</returns>
        public IEnumerable<T> Selecting(Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
        
        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>, providing the element and its zero-based index, and then yields the element.
        /// This allows for side effects during enumeration while still returning the original sequence.
        /// </summary>
        /// <param name="action">The <see cref="Action{T1,T2}"/> delegate to perform on each element, where T1 is the element and T2 is its index.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the original elements after the action has been performed.</returns>
        public IEnumerable<T> Selecting(Action<T, uint> action)
        {
            uint i = 0;
            foreach (var item in source)
            {
                action(item, i++);
                yield return item;
            }
        }
        
        /// <summary>
        /// Performs the specified action on elements of the <see cref="IEnumerable{T}"/> at specific zero-based indexes, and then yields the element.
        /// This allows for side effects during enumeration while still returning the original sequence.
        /// </summary>
        /// <param name="action">The <see cref="Action{T1,T2}"/> delegate to perform on the selected elements, where T1 is the element and T2 is its index.</param>
        /// <param name="indexes">A collection of zero-based indexes indicating which elements to apply the action to.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the original elements after the action has been performed.</returns>
        public IEnumerable<T> Selecting(Action<T, uint> action, IEnumerable<uint> indexes)
        {
            var set = new HashSet<uint>(indexes);
            uint i = 0;
            foreach (var item in source)
            {
                if (set.Contains(i))
                {
                    action(item, i);
                }
                i++;
                yield return item;
            }
        }
        
        #endregion
    }
}