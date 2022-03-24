using JetBrains.Annotations;

namespace Profile.Application.Features.UpdateProfile
{
    public class UpdateProfileRequestDto
    {
        [CanBeNull] public string FirstName { get; set; }
        [CanBeNull] public string LastName { get; set; }
    }
}