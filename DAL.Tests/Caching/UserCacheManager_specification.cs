namespace Boilerplate.DAL.Testing.Caching {
    using Machine.Fakes;
    using Machine.Specifications;
    using Boilerplate.Caching;
    using Boilerplate.Domain;
    using Boilerplate.Test;
    using Boilerplate.Test.EntityFramework;

    public class UserCacheManager_specification : Base_specification {
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

        [Subject("UserCacheManager specification")]
        public class when_caching_a_user {
            static User _user;

            Establish context = () => {
                using (var factory = new EFTestDataFactory(DataContext)) {
                    factory.Batch(x => {
                        _user = x.CreateUser(y =>
                            y.WithAccount = _account);
                    });
                }
            };

            Because of = () =>
                UserCacheManager.Put(_user);

            It should_be_retrievable_by_user_id = () =>
                UserCacheManager.Get(_user.Id)
                    .ShouldEqual(_user);
        }

        [Subject("UserCacheManager specification")]
        public class when_caching_a_collection_of_users {
            static User _user1, _user2, _user3;

            Establish context = () => {
                using (var factory = new EFTestDataFactory(DataContext)) {
                    factory.Batch(x => {
                        _user1 = x.CreateUser(y =>
                            y.WithAccount = _account);
                        _user2 = x.CreateUser(y =>
                            y.WithAccount = _account);
                        _user3 = x.CreateUser(y =>
                            y.WithAccount = _account);
                    });
                }
            };

            Because of = () =>
                UserCacheManager.Put(new[] { _user1, _user2, _user3 });

            It should_be_retrievable_by_the_account_id = () =>
                UserCacheManager.GetAllByAccountId(_user1.AccountId)
                    .ShouldContainOnly(_user1, _user2, _user3);

            It should_be_allow_each_user_to_be_retrieved_by_id = () => {
                UserCacheManager.Get(_user1.Id)
                    .ShouldEqual(_user1);
                UserCacheManager.Get(_user2.Id)
                    .ShouldEqual(_user2);
                UserCacheManager.Get(_user3.Id)
                    .ShouldEqual(_user3);
            };
        }

        [Subject("UserCacheManager specificaiton")]
        public class when_getting_a_collection_of_users {
            static User _user1, _user2, _user3;

            Establish context = () => {
                using (var factory = new EFTestDataFactory(DataContext)) {
                    factory.Batch(x => {
                        _user1 = x.CreateUser(y =>
                            y.WithAccount = _account);
                        _user2 = x.CreateUser(y =>
                            y.WithAccount = _account);
                        _user3 = x.CreateUser(y =>
                            y.WithAccount = _account);
                    });
                }

                UserCacheManager.Put(new[] { _user1, _user2, _user3 });
            };


            [Subject("UserCacheManager specificaiton, when getting a collection of users")]
            public class and_one_of_the_applicaitons_is_missing {
                Establish context = () =>
                    UserCacheManager.Remove(_user2);

                It should_return_an_empty_collection = () =>
                    UserCacheManager.GetAllByAccountId(_user1.AccountId)
                        .ShouldBeEmpty();
            }

            [Subject("UserCacheManager specificaiton, when getting a collection of users")]
            public class and_the_applicaitons_for_the_account_have_been_removed {
                Establish context = () =>
                    UserCacheManager.RemoveAllByAccountId(_user1.AccountId);

                It should_return_an_empty_collection = () =>
                    UserCacheManager.GetAllByAccountId(_user1.AccountId)
                        .ShouldBeEmpty();
            }
        }

        [Subject("UserCacheManager specification")]
        public class when_removing_a_user {
            static User _user;

            Establish context = () => {
                using (var factory = new EFTestDataFactory(DataContext)) {
                    factory.Batch(x => {
                        _user = x.CreateUser(y =>
                            y.WithAccount = _account);
                    });
                }

                UserCacheManager.Put(_user);
            };

            Because of = () =>
                UserCacheManager.Remove(_user);

            It should_no_longer_be_available_from_the_cache = () =>
                UserCacheManager.Get(_user.Id)
                    .ShouldBeNull();
        }
    }
}
