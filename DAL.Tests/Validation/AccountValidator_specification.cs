namespace Boilerplate.DAL.Testing.Validation {
    using FluentValidation.TestHelper;
    using Machine.Specifications;
    using Boilerplate.Domain;
    using Boilerplate.Validation;

    public class AccountValidator_specification {
        static Account _account;
        static AccountValidator _validator;

        Establish context = () => {
            _account = new Account {
                Name = "Valid Name"
            };
            _validator = new AccountValidator();
        };

        [Subject("AccountValidator specification")]
        public class when_validating {
            It should_return_no_errors_if_everything_is_valid = () =>
                _validator.Validate(_account).IsValid
                    .ShouldBeTrue();

            [Subject("AccountValidator specification, when validating")]
            public class and_the_account_is_null {
                Establish context = () =>
                    _account = null;

                It should_return_an_error = () =>
                    _validator.Validate(_account).IsValid
                        .ShouldBeFalse();
            }

            [Subject("AccountValidator specification, when validating")]
            public class and_the_account_id_has_not_been_initialized {
                Establish context = () =>
                    _account.Id = 0;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.Id, _account);
            }

            [Subject("AccountValidator specification, when validating")]
            public class and_the_account_api_key_has_not_been_initialized {
                Establish context = () =>
                    _account.Key = null;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.Key, _account);
            }

            [Subject("AccountValidator specification, when validating")]
            public class and_the_name_is_greater_than_128_characters {
                const string STRING_129 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse dolor mauris, tincidunt sit amet pellentesque sed cras amet.";

                Establish context = () =>
                    _account.Name = STRING_129;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.Name, _account);
            }

            [Subject("AccountValidator specification, when validating")]
            public class and_the_account_is_not_active {
                Establish context = () =>
                    _account.IsActive = false;

                It should_return_an_error = () =>
                    _validator.ShouldHaveValidationErrorFor(x => x.IsActive, _account);
            }
        }
    }
}
