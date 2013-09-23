namespace Boilerplate.DAL.Testing.Domain {
    using System;
    using System.Linq;
    using Machine.Specifications;
    using Boilerplate.Domain;
    using Boilerplate.Test;
    using Boilerplate.Test.EntityFramework;

    public class Account_specification : Base_specification {
        static Account _account;

        Establish context = () =>
            _account = new Account();

        [Subject("Account specification")]
        public class by_default {
            It should_initialize_the_date_created = () =>
                _account.DateCreated.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

            It should_initialized_the_current_id = () =>
                _account.Id
                    .ShouldNotEqual(0);

            It should_initialize_the_key = () =>
                _account.Key.ShouldNotBeEmpty();

            It should_be_active = () =>
                _account.IsActive.ShouldBeTrue();
        }

        [Subject("Account specification")]
        public class when_creating_a_new_account {
            static Account _account;

            Because of = () => {
                _account = Account.Create();
                Scope.Commit();
            };

            It should_add_the_account_to_the_context =()=>
                DataContext.Set<Account>()
                    .Any(x => x.Id == _account.Id)
                    .ShouldBeTrue();
        }

        [Subject("Account specification")]
        public class when_getting_an_account_by_key {
            static string _key;
            static Account _result;

            Establish context =()=>
                _key = Guid.NewGuid().ToString().Replace("-","");

            Because of =()=>
                _result = Account.GetByKey(_key);

            [Subject("Account specification, when getting an account by key")]
            public class and_no_accounts_match_the_supplied_value {
                It should_return_a_null_result = () =>
                    _result.ShouldBeNull();
            }

            [Subject("Account specification, when getting an account by key")]
            public class and_a_matching_account_exists {
                static Account _account;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _account = x.CreateAccount(y =>
                                y.Key = _key);
                        });
                    }
                };

                It should_return_the_correct_account = () =>
                    _result.Id
                        .ShouldEqual(_account.Id);
            }
        }

        [Subject("Account specification")]
        public class when_getting_an_account_by_id {
            static long _id;
            static Account _result;

            Establish context = () =>
                _id = 100;

            Because of = () =>
                _result = Account.GetById(_id);

            [Subject("Account specification, when getting an account by id")]
            public class and_no_accounts_match_the_supplied_value {
                It should_return_a_null_result = () =>
                    _result.ShouldBeNull();
            }

            [Subject("Account specification, when getting an account by id")]
            public class and_a_matching_account_exists {
                static Account _account;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _account = x.CreateAccount();
                        });
                    }
                    _id = _account.Id;
                };

                It should_return_the_correct_account = () =>
                    _result.Id
                        .ShouldEqual(_account.Id);
            }
        }
    }
}
