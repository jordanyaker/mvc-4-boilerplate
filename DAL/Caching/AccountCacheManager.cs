namespace Boilerplate.Caching {
    using System;
    using Microsoft.Practices.ServiceLocation;
    using StackExchange.Profiling;
    using Boilerplate.Domain;

    public static class AccountCacheManager {
        // ------------------------------------------------------------------------------
        // Constructor
        // ------------------------------------------------------------------------------
        static AccountCacheManager() {
            try {
                _manager = ServiceLocator.Current.GetInstance<ICacheManager>();
            } catch {
                // Cache manager has not been initialized.
            }
        }

        // ------------------------------------------------------------------------------
        // Constants
        // ------------------------------------------------------------------------------
        const string ENTITY_BY_ID_PATTERN = ":{0}";
        const string ENTITY_BY_KEY_PATTERN = "Account:{0}";

        // ------------------------------------------------------------------------------
        // Fields
        // ------------------------------------------------------------------------------
        static ICacheManager _manager;

        // ------------------------------------------------------------------------------
        // Methods
        // ------------------------------------------------------------------------------
        public static Account Get(long id) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountCacheManager.Get")) {
#endif
                if (_manager == null) {
                    return null;
                }

                string key = ENTITY_BY_ID_PATTERN.FormatWith(id);
                return _manager.Get<Account>(key);
#if DEBUG
            }
#endif
        }
        public static Account Get(string apiKey) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountCacheManager.Get")) {
#endif
                if (_manager == null) {
                    return null;
                }

                string key = ENTITY_BY_KEY_PATTERN.FormatWith(apiKey);
                return _manager.Get<Account>(key);
#if DEBUG
            }
#endif
        }
        public static void Put(Account entity) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountCacheManager.Put")) {
#endif
                if (_manager == null) {
                    return;
                }

                string key = ENTITY_BY_ID_PATTERN.FormatWith(entity.Id);
                _manager.Put<Account>(key, entity, TimeSpan.FromMinutes(5));
                
                string apiTag = ENTITY_BY_KEY_PATTERN.FormatWith(entity.Key);
                _manager.Put<Account>(apiTag, entity, TimeSpan.FromMinutes(5));
#if DEBUG
            }
#endif
        }
        public static void Remove(Account entity) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountCacheManager.Remove")) {
#endif
                if (_manager == null) {
                    return;
                }

                string key = ENTITY_BY_ID_PATTERN.FormatWith(entity.Id);
                _manager.Remove<Account>(key);

                string apiTag = ENTITY_BY_KEY_PATTERN.FormatWith(entity.Key);
                _manager.Remove<Account>(apiTag);

                UserCacheManager.RemoveAllByAccountId(entity.Id);
#if DEBUG
            }
#endif
        }
    }
}
