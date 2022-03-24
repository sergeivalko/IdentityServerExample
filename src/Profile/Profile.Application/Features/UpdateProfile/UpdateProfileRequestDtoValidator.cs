using FluentValidation;

namespace Profile.Application.Features.UpdateProfile
{
    public class UpdateProfileRequestDtoValidator : AbstractValidator<UpdateProfileRequestDto>
    {
        public UpdateProfileRequestDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .MinimumLength(1)
                .MaximumLength(45);

            RuleFor(x => x.LastName)
                .MinimumLength(1)
                .MaximumLength(45);
        }
    }
}