using Demo.Infrastructure.RequestModel;
using FluentValidation;

namespace Demo.Infrastructure.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.FirstName).NotNull().NotEmpty().WithMessage("First name can not be null");
            RuleFor(u => u.LastName).NotNull().NotEmpty().WithMessage("Last name can not be null");
            
            RuleFor(u => u.Email).NotNull().NotEmpty().WithMessage("Email address is required").EmailAddress().WithMessage("A valid email is required");
            
            RuleFor(u => u.Password).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage("Your password cannot be empty")
                   .MinimumLength(6).WithMessage("Your password length must be at least 6 characters.")
                   .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                   .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        }
    }
}
