using System;
using MediatR;

namespace Profile.Application.Features.GetProfile
{
    public record GetProfileQuery(Guid ProfileId) : IRequest<GetProfileResult>;
}