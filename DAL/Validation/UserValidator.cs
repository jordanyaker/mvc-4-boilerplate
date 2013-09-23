namespace Boilerplate.Validation {
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using FluentValidation.Validators;
    using NCommon.Data.EntityFramework;
    using Boilerplate.Domain;

    public class UserValidator : AbstractValidator<User> {
        public UserValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                    .WithMessage("40002;{PropertyName};{PropertyName} is required.");

            RuleFor(x => x.AccountId)
                .NotEmpty()
                    .WithMessage("40002;{PropertyName};{PropertyName} is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("40002;{PropertyName};{PropertyName} is required.")
                .Length(0, 254)
                    .WithMessage("40003;{PropertyName};{PropertyName} must be less than {MaxLength} characters.")
                .EmailAddress()
                    .WithMessage("40009;{PropertyName};{PropertyName} is not a valid email address.");
            
            RuleFor(x => x.Email)
                .Must(HaveAUniqueEmailAddress)
                    .WithMessage("40007;{PropertyName};The {PropertyName} \"{PropertyValue}\" is currently in use.")
                .When(x => string.IsNullOrEmpty(x.Email) == false);

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("40002;{PropertyName};{PropertyName} is required.")
                .Length(0, 100)
                    .WithMessage("40003;{PropertyName};{PropertyName} must be less than {MaxLength} characters.");

            RuleFor(x => x.PasswordSalt)
                .NotEmpty()
                    .WithMessage("40002;{PropertyName};{PropertyName} is required.");
           
            RuleFor(x => x.PasswordValue)
                .NotEmpty()
                    .WithMessage("40002;{PropertyName};{PropertyName} is required.");

            RuleFor(x => x.IsActive)
                .Equal(true)
                    .WithMessage("40008;{PropertyName};{PropertyName} is not true.");

            RuleFor(x => x.AccountId)
                .Must(BeAnActiveAccount)
                    .WithMessage("40008;{PropertyName};{PropertyName} is not for an active Account.")
                .When(x => x.AccountId != 0);
        }
        public override ValidationResult Validate(User instance) {
            if (instance == null) {
                return new ValidationResult(new[] { 
                    new ValidationFailure("Id", "40001;Id;The system was unable to locate the specified Category.")
                });
            }

            return base.Validate(instance);
        }
        private bool BeAnActiveAccount(User user, long accountId) {
            if (user.WithAccount != null) {
                return user.WithAccount.IsActive;
            }

            var account = new EFRepository<Account>()
                .FirstOrDefault(x => x.Id == accountId);

            return account != null && account.IsActive;
        }
        private bool HaveAUniqueEmailAddress(User user, string email, PropertyValidatorContext context) {
            var other = new EFRepository<User>()
                .FirstOrDefault(x =>
                    x.Id != user.Id &&
                    x.IsActive &&
                    x.Email == email.ToLower());

            if (other != null) {
                context.MessageFormatter.AppendArgument("PropertyValue", email);
            }

            return (other == null);
        }
    }
}
