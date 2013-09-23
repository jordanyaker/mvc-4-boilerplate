namespace System.Runtime.Caching {
    public static class CachingExtensions {
        public static string BuildFullKey<T>(this object key) {
            if (key == null)
                return typeof(T).Name;
            return typeof(T).Name + key;
        }
    }
}
