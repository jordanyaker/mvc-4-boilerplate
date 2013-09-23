namespace Boilerplate.DAL.Testing.Caching {
    using Machine.Fakes;
    using Machine.Specifications;
    using Boilerplate.Caching;
    using Boilerplate.Domain;
    using Boilerplate.Test;
    using Boilerplate.Test.EntityFramework;

    public class AccountCacheManager_specification : Base_specification {
        static Account _account;

        Establish context = () => {
            Locator
                .WhenToldTo(x => x.GetInstance<ICacheManager>())
                .Return(new MemoryCacheManager());

            using (var factory = new EFTestDataFactory(DataContext)) {
                factory.Batch(x => {
                    _account = x.CreateAccount();
                });
            }
        };

        [Subject("AccountCacheManager specification")]
        public class when_caching_an_account {
            Because of=()=>
                AccountCacheManager.Put(_account);

            It should_be_retrievable_by_account_id = () =>
                AccountCacheManager.Get(_account.Id)
                    .ShouldEqual(_account);

            It should_be_retrievable_by_api_key = () =>
                AccountCacheManager.Get(_account.Key)
                    .ShouldEqual(_account);
        }

        [Subject("AccountCacheManager specification")]
        public class when_removing_an_account {
            Establish context = () =>
                AccountCacheManager.Put(_account);

            Because of = () =>
                AccountCacheManager.Remove(_account);

            It should_no_longer_be_available_from_the_cache = () =>
                AccountCacheManager.Get(_account.Id)
                    .ShouldBeNull();
        }
    }
}
