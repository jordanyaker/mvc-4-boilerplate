namespace Boilerplate.Caching {
    using System;
    using System.Collections.Generic;

    public interface ICacheManager {
        void Clear();
        T Get<T>(string key = null);
        void Put<T>(string key, T instance, TimeSpan slidingExpiration);
        void Remove<T>(string key);
    }
}
