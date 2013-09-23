namespace Boilerplate.Caching {
    using System;
    using System.Linq;
    using System.Runtime.Caching;

    public class MemoryCacheManager : ICacheManager {
        // -------------------------------------------------------------------------------------
        // Fields
        // -------------------------------------------------------------------------------------
        MemoryCache _cache;

        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        public MemoryCacheManager() {
            _cache = MemoryCache.Default;
        }

        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        public void Clear() {
            var entries = _cache.ToArray();

            foreach (var entry in entries) {
                _cache.Remove(entry.Key);
            }
        }
        public T Get<T>(string key) {
            string full = key.BuildFullKey<T>();

            var value = (T)_cache.Get(full);
            if (value == null) {
                return default(T);
            }

            return value;
        }
        public void Put<T>(string key, T instance, TimeSpan slidingExpiration) {
            var policy = new CacheItemPolicy {
                SlidingExpiration = slidingExpiration
            };

            _cache.Set(key.BuildFullKey<T>(), instance, policy);
        }
        public void Remove<T>(string key) {
            _cache.Remove(key.BuildFullKey<T>());
        }
    }
}
