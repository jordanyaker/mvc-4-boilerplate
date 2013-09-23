namespace Boilerplate.Common.Tests.Caching {
    using Machine.Specifications;
    using Boilerplate.Caching;
    using System;

    public class MemoryCacheManager_specification {
        static MemoryCacheManager _memoryCacheManager;

        Establish context = () =>
            _memoryCacheManager = new MemoryCacheManager();

        [Subject("MemoryCacheManager specification")]
        public class when_adding_an_entry_to_the_cache {
            static string _instance = "cached item";
            static TimeSpan _expiration = TimeSpan.FromSeconds(2);

            Because of = () =>
                _memoryCacheManager.Put(null, _instance, _expiration);

            It should_store_the_item_in_the_cache = () =>
                _memoryCacheManager.Get<string>(null)
                    .ShouldEqual(_instance);
        }

        [Subject("MemoryCacheManager specification")]
        public class when_an_item_is_in_the_cache {
            static string _instance = "cached item";
            static TimeSpan _expiration = TimeSpan.FromSeconds(2);

            Establish context = () =>
                _memoryCacheManager.Put(null, _instance, _expiration);

            [Subject("MemoryCacheManager specification, when an item is in the cache")]
            public class and_the_expiration_time_has_passed {
                Because of = () => System.Threading.Thread.Sleep(2000);

                It should_no_longer_find_the_item_in_the_cache = () =>
                    _memoryCacheManager.Get<string>(null)
                        .ShouldBeNull();
            }

            [Subject("MemoryCacheManager specification, when an item is in the cache")]
            public class and_the_item_is_removed_from_the_cache {
                Because of = () => _memoryCacheManager.Remove<string>(null);

                It should_no_longer_find_the_item_in_the_cache = () =>
                    _memoryCacheManager.Get<string>(null)
                        .ShouldBeNull();
            }

            [Subject("MemoryCacheManager specification, when an item is in the cache")]
            public class and_the_cache_is_cleared {
                Because of = () => _memoryCacheManager.Clear();

                It should_no_longer_find_the_item_in_the_cache = () =>
                    _memoryCacheManager.Get<string>(null)
                        .ShouldBeNull();
            }
        }
    }
}
