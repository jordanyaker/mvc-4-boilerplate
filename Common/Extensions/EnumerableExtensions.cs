namespace System.Collections.Generic {
    using System;
    using System.Linq;

    public static class EnumerableExtensions {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            Array.ForEach(collection.ToArray(), action);
        }
    }
}