using FluentValidation;

namespace Auth.Application.Features.Commands.CreateAccount
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6);
        }
    }
}