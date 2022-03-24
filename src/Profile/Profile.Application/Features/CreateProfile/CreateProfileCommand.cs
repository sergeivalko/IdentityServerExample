using System;
using MediatR;

namespace Profile.Application.Features.CreateProfile
{
    public record CreateProfileCommand(Guid AccountId) : INotification;
}