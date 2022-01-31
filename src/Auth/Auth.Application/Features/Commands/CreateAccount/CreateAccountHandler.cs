using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Features.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, CreateAccountResult>
    {
        private readonly UserManager<User> _userManager;

        public CreateAccountHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateAccountResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var userDto = request.AccountRequest;

            var userExists = await _userManager.FindByEmailAsync(userDto.Email);

            if (userExists != null)
            {
                throw new Exception("User exists");
            }
            
            var user = new User()
            {
                Email = userDto.Email,
                Id = Guid.NewGuid(),
                UserName = userDto.Username,
                NormalizedEmail = userDto.Email,
                NormalizedUserName = userDto.Username,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Errors.Any()) return new CreateAccountResult();
            
            var errors = result.Errors.First();
            throw new Exception(errors.Description);
        }
    }
}