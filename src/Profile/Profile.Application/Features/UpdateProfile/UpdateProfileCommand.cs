using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MediatR;

namespace Profile.Application.Features.UpdateProfile
{
    public record UpdateProfileCommand(Guid ProfileId, [CanBeNull] string FirstName, [CanBeNull] string LastName,
        [CanBeNull] IEnumerable<byte> FileData) : INotification;
}
