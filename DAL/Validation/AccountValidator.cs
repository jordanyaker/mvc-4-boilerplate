namespace Boilerplate.Validation {
    using FluentValidation;
    using FluentValidation.Results;
    using Boilerplate.Domain;

    public class AccountValidator : AbstractValidator<Account> {
        public AccountValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                    .WithMessage("10002;{PropertyName};{PropertyName} has not been initialized.");

            RuleFor(x => x.Key)
                .NotEmpty()
                    .WithMessage("10002;{PropertyName};{PropertyName} has not been initialized.");

            RuleFor(x => x.Name)
                .Length(0, 128)
                    .WithMessage("10003;{PropertyName};{PropertyName} must be less than {MaxLength} characters.");

            RuleFor(x => x.IsActive)
                .Equal(true)
                    .WithMessage("10008;{PropertyName};{PropertyName} is not true.");
        }
        public override ValidationResult Validate(Account instance) {
            if (instance == null) {
                var error = new ValidationFailure("Id", "10001;Id;The system was unable to locate the specified Account.");
                return new ValidationResult(new[] { error });
            }

            return base.Validate(instance);
        }
    }
}
