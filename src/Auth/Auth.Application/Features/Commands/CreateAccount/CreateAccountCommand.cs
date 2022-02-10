using MediatR;

namespace Auth.Application.Features.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<CreateAccountResult>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public A A { get; set; }
    }


    public class A
    {
        public string Test { get; set; }
    }
}