using MediatR;

namespace Auth.Application.Features.Commands.CreateAccount
{
    public record CreateAccountCommand(string Username, string Email, string Password) : IRequest<CreateAccountResult>;
}