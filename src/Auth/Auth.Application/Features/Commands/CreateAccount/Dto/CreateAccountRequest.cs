namespace Auth.Application.Features.Commands.CreateAccount.Dto
{
    public class CreateAccountRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}