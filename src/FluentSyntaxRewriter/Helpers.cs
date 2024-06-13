using System;
using System.Collections.Generic;

namespace FluentSyntaxRewriter
{
    internal static class Helpers
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var seenKeys = new HashSet<TKey>(comparer);
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
