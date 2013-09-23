namespace Boilerplate.Caching {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.ServiceLocation;
    using StackExchange.Profiling;
    using Boilerplate.Domain;

    public static class UserCacheManager {
        // ------------------------------------------------------------------------------
        // Constructor
        // ------------------------------------------------------------------------------
        static UserCacheManager() {
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
        const string ENTITY_BY_ACCOUNT_PATTERN = "User:Account:{0}";

        // ------------------------------------------------------------------------------
        // Fields
        // ------------------------------------------------------------------------------
        static ICacheManager _manager;

        // ------------------------------------------------------------------------------
        // Methods
        // ------------------------------------------------------------------------------
        public static User Get(long id) {
#if DEBUG
            using (MiniProfiler.Current.Step("UserCacheManager.Get")) {
#endif
                if (_manager == null) {
                    return null;
                }

                string key = ENTITY_BY_ID_PATTERN.FormatWith(id);
                return _manager.Get<User>(key);
#if DEBUG
            }
#endif
        }
        public static IEnumerable<User> GetAllByAccountId(long accountId) {
#if DEBUG
            using (MiniProfiler.Current.Step("UserCacheManager.GetAllByAccountId")) {
#endif
                if (_manager == null) {
                    return null;
                }

                string key = ENTITY_BY_ACCOUNT_PATTERN.FormatWith(accountId);
                List<User> results = new List<User>();

                var ids = _manager.Get<IEnumerable<long>>(key);
                if (ids != null) {
                    foreach (var id in ids) {
                        User result = Get(id);

                        if (result == null) {
                            return new User[0];
                        }

                        results.Add(result);
                    }
                }

                return results;
#if DEBUG
            }
#endif
        }
        public static void Put(User entity) {
#if DEBUG
            using (MiniProfiler.Current.Step("UserCacheManager.Put")) {
#endif
                if (_manager == null) {
                    return;
                }

                // Store the primary key as referenced by the ID.
                string key = ENTITY_BY_ID_PATTERN.FormatWith(entity.Id);

                _manager.Put<User>(key, entity, TimeSpan.FromMinutes(5));
#if DEBUG
            }
#endif
        }
        public static void Put(IEnumerable<User> entities) {
#if DEBUG
            using (MiniProfiler.Current.Step("UserCacheManager.Put")) {
#endif
                if (_manager == null) {
                    return;
                }

                if (entities.Any()) {
                    entities.ForEach(entity => Put(entity));

                    var accountId = entities.First()
                        .AccountId;
                    var ids = entities.Select(x => x.Id)
                        .ToArray();

                    string key = ENTITY_BY_ACCOUNT_PATTERN.FormatWith(accountId);
                    _manager.Put<IEnumerable<long>>(key, ids, TimeSpan.FromMinutes(5));
                }
#if DEBUG
            }
#endif
        }
        public static void Remove(User entity) {
#if DEBUG
            using (MiniProfiler.Current.Step("UserCacheManager.Remove")) {
#endif
                if (_manager == null) {
                    return;
                }

                string key = ENTITY_BY_ID_PATTERN.FormatWith(entity.Id);
                _manager.Remove<User>(key);
#if DEBUG
            }
#endif
        }
        public static void RemoveAllByAccountId(long accountId) {
#if DEBUG
            using (MiniProfiler.Current.Step("UserCacheManager.RemoveAllByAccountId")) {
#endif
                if (_manager == null) {
                    return;
                }

                string key = ENTITY_BY_ACCOUNT_PATTERN.FormatWith(accountId);
                _manager.Remove<IEnumerable<long>>(key);
#if DEBUG
            }
#endif
        }
    }
}
