using System;
using System.Collections.Generic;

namespace FluentSyntaxRewriter
{
    /// <summary>
    /// Provides helper methods for the library.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Returns distinct elements from a sequence by using a specified key selector.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by keySelector.
        /// </typeparam>
        /// <param name="source">
        /// The sequence to remove duplicate elements from.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract the key for each element.
        /// </param>
        /// <param name="comparer">
        /// An equality comparer to compare values.
        /// </param>
        /// <returns>
        /// An IEnumerable that contains distinct elements from the source sequence.
        /// </returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var seenKeys = new HashSet<TKey>(comparer);

            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
            }
        }
    }
}
