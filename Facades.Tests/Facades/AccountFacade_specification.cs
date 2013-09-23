namespace Boilerplate.Facades.Testing {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Machine.Specifications;
    using Boilerplate.Contexts;
    using Boilerplate.Domain;
    using Boilerplate.Facades;
    using Boilerplate.Test;
    using Boilerplate.Test.EntityFramework;

    public class AccountFacade_specification : Base_specification {
        static AccountFacade _accountFacade;

        Establish context = () => {
            _accountFacade = new AccountFacade();
        };

        [Subject("AccountFacade specification")]
        public class when_canceling_an_account {
            static long _accountId;
            static FacadeResult _result;

            Establish context = () => {
                _accountId = 1234L;
            };

            Because of = () => {
                _result = _accountFacade.CancelAccount(_accountId);
                
                DataContext.ChangeTracker.Entries().ForEach(x => 
                    x.Reload());
            };

            [Subject("AccountFacade specification, when canceling an account")]
            public class and_the_account_cannot_be_found {
                It should_return_an_error_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Error);
                };
            }

            [Subject("AccountFacade specification, when canceling an account")]
            public class and_a_valid_account_exists {
                static Account _account;
                static User _user1, _user2, _user3;

                Establish context =()=> {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _account = x.CreateAccount();

                            _user1 = x.CreateUser(y =>
                                y.WithAccount = _account);
                            _user2 = x.CreateUser(y =>
                                y.WithAccount = _account);
                            _user3 = x.CreateUser(y =>
                                y.WithAccount = _account);

                            x.CreateAccount();
                            x.CreateAccount();

                            x.CreateUser();
                            x.CreateUser();
                            x.CreateUser();
                        });
                    }

                    _accountId = _account.Id;
                };

                It should_return_a_successful_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Success);
                };

                It should_set_the_account_as_inactive = () => {
                    DataContext.Entry<Account>(_account)
                        .Reload();
                    
                    _account.IsActive
                        .ShouldBeFalse();
                };

                It should_only_deactivate_the_specified_account = () => {
                    DataContext.Set<Account>()
                        .Where(x => x.Id != _account.Id)
                        .All(x => x.IsActive)
                        .ShouldBeTrue();
                };

                It should_set_all_users_on_the_account_as_inactive = () => {
                    DataContext.Set<User>()
                        .Where(x => x.AccountId == _accountId)
                        .ShouldEachConformTo(x => x.IsActive == false);
                };

                It should_only_deactivate_users_on_the_account = () => {
                    DataContext.Set<User>()
                        .Where(x => 
                            x.Id != _user1.Id &&
                            x.Id != _user2.Id &&
                            x.Id != _user3.Id)
                        .All(x => x.IsActive)
                        .ShouldBeTrue();
                };

                [Subject("AccountFacade specification, when canceling an account, and a valid account exists")]
                public class and_the_account_is_not_active {
                    Establish context = () => {
                        using (var factory = new EFTestDataFactory(DataContext)) {
                            factory.Batch(x => {
                                _account = x.CreateAccount(y =>
                                    y.IsActive = false);
                            });
                        }

                        _accountId = _account.Id;
                    };

                    It should_return_an_error_result = () => {
                        _result.Type.ShouldEqual(FacadeResultTypes.Error);
                    };
                }
            }
        }
        [Subject("AccountFacade specification")]
        public class when_getting_an_account_by_key {
            static Account _result;
            static string _accountId;

            Establish context = () => {
                _accountId = Guid.NewGuid().ToString().Replace("-","");
            };

            Because of = () =>
                _result = _accountFacade.GetAccountByKey(_accountId);

            [Subject("AccountFacade specification, when getting an account by key")]
            public class and_no_accounts_exist {
                It should_return_a_null_value = () => {
                    _result.ShouldBeNull();
                };
            }

            [Subject("AccountFacade specification, when getting an account by key")]
            public class and_accounts_exist {
                static Account _account1, _account2, _account3, _account4;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _account1 = x.CreateAccount();
                            _account2 = x.CreateAccount();
                            _account3 = x.CreateAccount();
                            _account4 = x.CreateAccount();
                        });
                    }

                    _accountId = _account3.Key;
                };

                It should_return_the_correct_account = () => {
                    _result.Id.ShouldEqual(_account3.Id);
                    _result.Key.ShouldEqual(_account3.Key);
                    _result.Name.ShouldEqual(_account3.Name);
                    _result.IsActive.ShouldEqual(_account3.IsActive);
                    _result.DateCreated.ShouldBeCloseTo(_account3.DateCreated, TimeSpan.FromSeconds(1));
                };
            }
        }
        [Subject("AccountFacade specification")]
        public class when_getting_an_account_by_id {
            static Account _result;
            static long _accountId;

            Establish context = () => {
                _accountId = 123L;
            };

            Because of = () =>
                _result = _accountFacade.GetAccountById(_accountId);

            [Subject("AccountFacade specification, when getting an account by id")]
            public class and_no_accounts_exist {
                It should_return_a_null_value = () => {
                    _result.ShouldBeNull();
                };
            }

            [Subject("AccountFacade specification, when getting an account by id")]
            public class and_accounts_exist {
                static Account _account1, _account2, _account3, _account4;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _account1 = x.CreateAccount();
                            _account2 = x.CreateAccount();
                            _account3 = x.CreateAccount();
                            _account4 = x.CreateAccount();
                        });
                    }

                    _accountId = _account3.Id;
                };

                It should_return_the_correct_account = () => {
                    _result.Id.ShouldEqual(_account3.Id);
                    _result.Key.ShouldEqual(_account3.Key);
                    _result.Name.ShouldEqual(_account3.Name);
                    _result.IsActive.ShouldEqual(_account3.IsActive);
                    _result.DateCreated.ShouldBeCloseTo(_account3.DateCreated, TimeSpan.FromSeconds(1));
                };
            }
        }
        [Subject("AccountFacade specification")]
        public class when_signing_up_a_new_account {
            static string _email = "name@company.com", _password = "password", _name = "Test User";
            static FacadeResult<User> _result;

            Because of = () =>
                _result = _accountFacade.SignUp(_name, _email, _password);

            [Subject("AccountFacade specification, when signing up a new account")]
            public class and_there_is_a_validation_error {
                static User _existing;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _existing = x.CreateUser(y =>
                                y.Email = _email);
                        });
                    }
                };

                It should_return_an_error_result = () =>
                    _result.Type
                        .ShouldEqual(FacadeResultTypes.Error);
            }

            [Subject("AccountFacade specification, when signing up a new account")]
            public class and_the_changes_were_successfully_saved {
                It should_return_a_successful_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Success);
                };

                It should_return_the_user_with_the_correct_details = () => {
                    _result.Data.Email
                        .ShouldEqual(_email);
                    _result.Data.IsPassword(_password)
                        .ShouldBeTrue();
                };

                It should_add_the_user_to_the_context = () => {
                    DataContext.Set<User>()
                        .Any(x => x.Id == _result.Data.Id)
                        .ShouldBeTrue();
                };

                It should_have_addeed_the_users_account_to_the_context = () => {
                    DataContext.Set<Account>()
                        .Any(x => x.Id == _result.Data.AccountId)
                        .ShouldBeTrue();
                };
            }
        }
        [Subject("AccountFacade specification")]
        public class when_signing_in_to_an_account {
            static string _email, _password;
            static FacadeResult<User> _result;

            Establish context = () => {
                _email = "name@company.com";
                _password = "password";
            };

            Because of = () =>
                _result = _accountFacade.SignIn(_email, _password);

            [Subject("AccountFacade specification, when signing in to an account")]
            public class and_the_specified_user_does_not_exist {
                It should_return_an_error_result = () =>
                    _result.Type
                        .ShouldEqual(FacadeResultTypes.Error);
            }

            [Subject("AccountFacade specification, when signing in to an account")]
            public class and_the_specified_user_exists {
                static User _user;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _user = x.CreateUser(y => {
                                y.Email = _email;
                                y.SetPassword("password");
                            });
                        });
                    }
                };

                [Subject("AccountFacade specification, when signing in to an account, and the specified user exists")]
                public class and_the_supplied_password_is_invalid {
                    Establish context = () =>
                        _password = "not password";

                    It should_return_an_error_result = () =>
                        _result.Type
                            .ShouldEqual(FacadeResultTypes.Error);
                }

                [Subject("AccountFacade specification, when signing in to an account, and the specified user exists")]
                public class and_the_supplied_password_is_valid {
                    Establish context = () =>
                        _password = "password";

                    It should_return_the_correct_user = () => {
                        _result.Data.Id.ShouldEqual(_user.Id);
                        _result.Data.AccountId.ShouldEqual(_user.AccountId);
                        _result.Data.Name.ShouldEqual(_user.Name);
                        _result.Data.Email.ShouldEqual(_user.Email);
                    };

                    It should_return_a_successful_result = () =>
                        _result.Type
                            .ShouldEqual(FacadeResultTypes.Success);
                }
            }
        }       
        [Subject("AccountFacade specification")]
        public class when_getting_a_user_by_id {
            static User _result;
            static long _userId;

            Establish context = () => {
                _userId = 12325436L;
            };

            Because of = () =>
                _result = _accountFacade.GetUserById(_userId);

            [Subject("AccountFacade specification, when getting a user by id")]
            public class and_no_users_exist {
                It should_return_a_null_object = () => {
                    _result.ShouldBeNull();
                };
            }

            [Subject("AccountFacade specification, when getting a user by id")]
            public class and_users_exist {
                static User _user1, _user2, _user3, _user4;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _user1 = x.CreateUser();
                            _user2 = x.CreateUser();
                            _user3 = x.CreateUser();
                            _user4 = x.CreateUser();
                        });
                    }

                    _userId = _user3.Id;
                };

                It should_return_the_correct_user = () => {
                    _result.Id.ShouldEqual(_user3.Id);
                    _result.AccountId.ShouldEqual(_user3.AccountId);
                    _result.Name.ShouldEqual(_user3.Name);
                    _result.Email.ShouldEqual(_user3.Email);
                };
            }
        }
        [Subject("AccountFacade specification")]
        public class when_getting_a_user_by_email {
            static User _result;
            static string _email;

            Establish context = () => {
                _email = "testing@company.com";
            };

            Because of = () =>
                _result = _accountFacade.GetUserByEmail(_email);

            [Subject("AccountFacade specification, when getting a user by email")]
            public class and_no_users_exist {
                It should_return_a_null_object = () => {
                    _result.ShouldBeNull();
                };
            }

            [Subject("AccountFacade specification, when getting a user by email")]
            public class and_users_exist {
                static User _user1, _user2, _user3, _user4;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _user1 = x.CreateUser();
                            _user2 = x.CreateUser();
                            _user3 = x.CreateUser();
                            _user4 = x.CreateUser();
                        });
                    }

                    _email = _user3.Email;
                };

                It should_return_the_correct_user = () => {
                    _result.Id.ShouldEqual(_user3.Id);
                    _result.AccountId.ShouldEqual(_user3.AccountId);
                    _result.Name.ShouldEqual(_user3.Name);
                    _result.Email.ShouldEqual(_user3.Email);
                };
            }
        }
        [Subject("AccountFacade specification")]
        public class when_updating_a_users_password {
            static FacadeResult<User> _result;
            static long _userId;
            static string _password;

            Establish context = () => {
                _userId = 12335L;
                _password = "new password";
            };

            Because of = () => {
                _result = _accountFacade.UpdateUserPassword(_userId, _password);
            };

            [Subject("AccountFacade specification, when updating a user's password")]
            public class and_the_user_cannot_be_found {
                It should_return_an_error_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Error);
                };
            }

            [Subject("AccountFacade specification, when updating a user's password")]
            public class and_the_update_was_successful {
                static User _user;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _user = x.CreateUser();
                        });
                    }

                    _userId = _user.Id;
                };

                It should_return_a_successful_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Success);
                };

                It should_have_updated_the_users_password = () =>
                    _result.Data.IsPassword(_password)
                        .ShouldBeTrue();
            }

            [Subject("AccountFacade specification, when updating a user's password")]
            public class and_the_changes_could_not_be_validated {
                static User _user;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _user = x.CreateUser(y =>
                                y.Email = "invalid email");
                        });
                    }

                    _userId = _user.Id;
                };

                It should_return_an_error_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Error);
                };
            }
        }
        [Subject("AccountFacade specification")]
        public class when_updating_a_users_profile {
            static FacadeResult<User> _result;
            static long _userId;
            static string _name, _email;

            Establish context = () => {
                _userId = 21325L;

                _name = "New Name";
                _email = "new@email.com";
            };

            Because of = () => {
                _result = _accountFacade.UpdateUserProfile(_userId, _name, _email);
            };

            [Subject("AccountFacade specification, when updating a user's profile")]
            public class and_the_user_cannot_be_found {
                It should_return_an_error_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Error);
                };
            }

            [Subject("AccountFacade specification, when updating a user's profile")]
            public class and_the_update_was_successful {
                static User _user;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _user = x.CreateUser();
                        });
                    }

                    _userId = _user.Id;
                };

                It should_return_a_successful_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Success);
                };

                It should_have_updated_the_users_name = () =>
                    _result.Data.Name
                        .ShouldEqual(_name);

                It should_have_updated_the_users_profile = () =>
                    _result.Data.Email
                        .ShouldEqual(_email);
            }

            [Subject("AccountFacade specification, when updating a user's profile")]
            public class and_an_invalid_value_was_supplied {
                static User _user;

                Establish context = () => {
                    using (var factory = new EFTestDataFactory(DataContext)) {
                        factory.Batch(x => {
                            _user = x.CreateUser();
                        });
                    }

                    _email = "definitely not an email address";

                    _userId = _user.Id;
                };

                It should_return_an_error_result = () => {
                    _result.Type.ShouldEqual(FacadeResultTypes.Error);
                };
            }
        }
    }
}
