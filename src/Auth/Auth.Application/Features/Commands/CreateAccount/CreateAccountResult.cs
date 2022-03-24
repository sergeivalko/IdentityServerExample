using System;
using MediatR;

namespace Auth.Application.Features.Commands.CreateAccount
{
    public record CreateAccountResult(Guid AccountId) : IRequest;
}