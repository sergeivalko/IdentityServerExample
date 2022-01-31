using Auth.Application.Features.Commands.CreateAccount.Dto;
using MediatR;

namespace Auth.Application.Features.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<CreateAccountResult>
    {
        public CreateAccountCommand(CreateAccountRequest request)
        {
            AccountRequest = request;
        }
        
        public CreateAccountRequest AccountRequest { get; set; }
    }
}