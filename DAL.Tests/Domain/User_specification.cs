namespace Boilerplate.DAL.Testing.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Machine.Specifications;
    using Boilerplate.Domain;
    using Boilerplate.Test;
    using Boilerplate.Test.EntityFramework;

    public class User_specification : Base_specification {
        static Account _account;

        Establish context = () => {
            using (var factory = new EFTestDataFactory(DataContext)) {
                factory.Batch(x => {
                    _account = x.CreateAccount();
                });
            }
        };

        [Subject("User specification")]
        public class by_default {
            static Exception _exception;
            static User _user;

            Because of = () =>
                _exception = Catch.Exception(() => 
                    _user = new User());

            It should_work_without_issues = () =>
                _exception.ShouldBeNull();

            It should_mark_the_user_as_active = () =>
                _user.IsActive.ShouldBeTrue();

            It should_initialize_the_date_created = () =>
                _user.DateCreated.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Subject("User specification")]
        public class when_creating_a_new_user_with_an_account {
            static User _user;
            static string _email, _name, _password;

            Establish context = () => {
                _name = "Test User";
                _email = "name@company.com";
                _password = "password";
            };

            Because of = () => {
                _user = User.Create(_account.Id, _email, _name, _password);
                Scope.Commit();
            };

            It should_save_the_new_user = () =>
                DataContext.Set<User>()
                    .Any(x => x.Id == _user.Id)
                    .ShouldBeTrue();

            It should_set_the_account_correctly = () =>
                _user.AccountId.ShouldEqual(_account.Id);

            It should_set_the_email_address_correctly = () =>
                _user.Email.ShouldEqual(_email);

            It should_set_the_name_correctly = () =>
                _user.Name.ShouldEqual(_name);

            It should_set_the_password_correctly = () => {
                _user.PasswordSalt.ShouldNotBeEmpty();
                _user.PasswordValue.ShouldNotBeEmpty();
                _user.PasswordValue.ShouldNotEqual(_password);
            };
        }

        [Subject("User specification")]
        public class when_creating_a_new_user_without_an_account {
            static User _user;
            static string _email, _name, _password;

            Establish context = () => {
                _name = "Test User";
                _email = "name@company.com";
                _password = "password";
            };

            Because of = () => {
                _user = User.Create(_email, _name, _password);
                Scope.Commit();
            };

            It should_save_the_new_user = () =>
                DataContext.Set<User>()
                    .Any(x => x.Id == _user.Id)
                    .ShouldBeTrue();

            It should_save_the_new_account = () =>
                DataContext.Set<Account>()
                    .Any(x => x.Id == _user.AccountId)
                    .ShouldBeTrue();

            It should_set_the_email_address_correctly = () =>
                _user.Email.ShouldEqual(_email);

            It should_set_the_name_correctly = () =>
                _user.Name.ShouldEqual(_name);

            It should_set_the_password_correctly = () => {
                _user.PasswordSalt.ShouldNotBeEmpty();
                _user.PasswordValue.ShouldNotBeEmpty();
                _user.PasswordValue.ShouldNotEqual(_password);
            };
        }

        [Subject("User specification")]
        public class when_getting_all_users_by_account_id {
            static User _existing1, _existing2, _existing3, _existing4;
            static IEnumerable<User> _results;

            Establish context = () => {
                using (var factory = new EFTestDataFactory(DataContext)) {
                    factory.Batch(x => {
                        _existing1 = x.CreateUser(y =>
                            y.Name = "test user 1");
                        _existing2 = x.CreateUser(y => {
                            y.WithAccount = _account;
                            y.Name = "test user 2";
                        });
                        _existing3 = x.CreateUser(y =>
                            y.Name = "test user 3");
                        _existing4 = x.CreateUser(y => {
                            y.WithAccount = _account;
                            y.Name = "test user 4";
                        });
                    });
                }
            };

            Because of = () => {
                _results = User.GetAllByAccountId(_account.Id);
            };

            It should_return_the_correct_users = () => {
                _results.Select(x => x.Id)
                    .ShouldContainOnly(_existing2.Id, _existing4.Id);
            };
        }

        [Subject("User specification")]
        public class when_getting_a_user_by_account_id_and_id {
            static User _result;
            static long _id;

            Because of = () =>
                _result = User.GetByAccountIdAndId(_account.Id, _id);

            [Subject("User specification, when getting a user by account id and id")]
            public class and_there_are_no_users {
                Establish context = () =>
                    _id = 100L;

                It should_return_a_null_result = () =>
                    _result.ShouldBeNull();
            }

            [Subject("User specification, when getting a user by account id and id")]
            public class and_there_are_matching_users {
                static User _expected;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                            _expected = x.CreateUser(y => {
                                y.WithAccount = _account;
                            });
                        });
                    }
                    _id = _expected.Id;
                };

                It should_return_the_correct_result = () => {
                    _result.Name.ShouldEqual(_expected.Name);
                    _result.Email.ShouldEqual(_expected.Email);
                    _result.AccountId.ShouldEqual(_expected.AccountId);
                };
            }
        }

        [Subject("User specification")]
        public class when_getting_a_user_by_email_address {
            static User _result;
            static string _email;

            Establish context = () =>
                _email = "name@company.com";

            Because of = () =>
                _result = User.GetByEmail(_email);

            [Subject("User specification, when getting a user by email address")]
            public class and_there_are_no_matching_users {
                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                        });
                    }
                };

                It should_return_null = () =>
                    _result.ShouldBeNull();
            }

            [Subject("User specification, when getting a user by email address")]
            public class and_there_is_a_matching_user {
                static User _expected;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser(y => {
                                y.WithAccount = _account;
                            });
                            x.CreateUser(y => {
                                y.WithAccount = _account;
                            });
                            x.CreateUser(y => {
                                y.WithAccount = _account;
                            });
                            _expected = x.CreateUser(y => {
                                y.WithAccount = _account;
                            });
                        });
                    }

                    _email = _expected.Email;
                };

                It should_return_the_correct_result = () => {
                    _result.Name.ShouldEqual(_expected.Name);
                    _result.Email.ShouldEqual(_expected.Email);
                    _result.AccountId.ShouldEqual(_expected.AccountId);
                };
            }
        }

        [Subject("User specification")]
        public class when_getting_a_user_by_id {
            static User _result;
            static long _id;

            Because of = () =>
                _result = User.GetById(_id);

            [Subject("User specification, when getting a user by id")]
            public class and_there_are_no_users {
                Establish context = () =>
                    _id = 100L;

                It should_return_a_null_result = () =>
                    _result.ShouldBeNull();
            }

            [Subject("User specification, when getting a user by id")]
            public class and_there_are_matching_users {
                static User _expected;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                            _expected = x.CreateUser(y => {
                                y.WithAccount = _account;
                            });
                        });
                    }
                    _id = _expected.Id;
                };

                It should_return_the_correct_result = () => {
                    _result.Name.ShouldEqual(_expected.Name);
                    _result.Email.ShouldEqual(_expected.Email);
                    _result.AccountId.ShouldEqual(_expected.AccountId);
                };
            }
        }

        [Subject("User specification")]
        public class when_setting_the_password {
            static User _user;
            static string _password, _salt;

            Establish context = () => {
                _password = "password";
                _salt = "salt";

                using (var factory = new EFTestDataFactory(DataContext)) {
                    factory.Batch(x =>
                        _user = x.CreateUser(y => {
                            y.PasswordSalt = _salt;
                            y.PasswordValue = _password;
                        }));
                }
            };

            Because of = () =>
                _user.SetPassword(_password);

            It should_regenerate_the_password_salt = () =>
                _user.PasswordSalt.ShouldNotEqual(_salt);

            It should_store_the_newly_hashed_password_value = () =>
                _user.PasswordSalt.ShouldNotEqual(_password);
        }

        [Subject("User specification")]
        public class when_checking_the_password {
            static User _user;
            static string _password;

            Establish context = () => {
                _password = "password";

                using (var factory = new EFTestDataFactory(DataContext)) {
                    factory.Batch(x =>
                        _user = x.CreateUser(y => {
                            y.SetPassword(_password);
                        }));
                }
            };

            It should_match_the_correct_password = () =>
                _user.IsPassword(_password).ShouldBeTrue();

            It should_not_match_the_incorrect_password = () =>
                _user.IsPassword("incorrect").ShouldBeFalse();

            It should_not_match_an_empty_string = () =>
                _user.IsPassword("").ShouldBeFalse();
        }
    }
}
