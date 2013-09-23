namespace Boilerplate.DAL.Testing.Validation {
    using System;
    using FluentValidation.TestHelper;
    using Machine.Specifications;
    using Boilerplate.Domain;
    using Boilerplate.Test;
    using Boilerplate.Test.EntityFramework;
    using Boilerplate.Validation;

    public class UserValidator_specification : Base_specification {
        static User _user;
        static UserValidator _validator;
        static Account _account;

        Establish context = () => {
            using (var testData = new EFTestDataFactory(DataContext)) {
                testData.Batch(x => {
                    _account = x.CreateAccount();
                });
            }

            _user = new User {
                AccountId = _account.Id,
                Name = "Valid Name",
                Email = "valid@company.com",
            };
            _user.SetPassword("password");

            _validator = new UserValidator();
        };

        [Subject("UserValidator specification")]
        public class when_validating {
            It should_not_return_any_errors_if_the_user_is_valid = () =>
                _validator.Validate(_user).IsValid
                    .ShouldBeTrue();

            [Subject("UserValidator specification, when validating")]
            public class and_the_user_is_null {
                Establish context = () =>
                    _user = null;

                It should_return_an_error = () =>
                    _validator.Validate(_user).IsValid
                        .ShouldBeFalse();
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_account_id_has_not_been_initialized {
                Establish context = () =>
                    _user.AccountId = 0;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.AccountId, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_user_id_has_not_been_initialized {
                Establish context = () =>
                    _user.Id = 0;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.Id, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_email_is_null {
                Establish context = () =>
                    _user.Email = null;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.Email, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_email_is_invalid {
                Establish context = () =>
                    _user.Email = "invalid email";

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.Email, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_user_does_not_have_a_unique_email {
                static User _existing;

                Establish context = () => {
                    using (var testData = new EFTestDataFactory(DataContext)) {
                        testData.Batch(x => {
                            _existing = x.CreateUser(y => {
                                y.WithAccount = _account;
                                y.Email = _user.Email;
                            });
                        });
                    }
                };

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.Email, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_password_salt_for_the_user_has_not_been_set {
                Establish context = () =>
                    _user.PasswordSalt = null;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.PasswordSalt, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_password_for_the_user_has_not_been_encoded {
                Establish context = () =>
                    _user.PasswordValue = null;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.PasswordValue, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_account_is_not_active {
                Establish context = () => {
                    _account.IsActive = false;
                    DataContext.SaveChanges();
                };

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.AccountId, _user);
            }

            [Subject("UserValidator specification, when validating")]
            public class and_the_account_is_invalid {
                Establish context = () =>
                    _user.AccountId = 123235;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.AccountId, _user);
            }
        }
    }
}
