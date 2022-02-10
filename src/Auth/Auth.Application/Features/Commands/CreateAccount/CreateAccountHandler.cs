using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StormShop.Common;

namespace Auth.Application.Features.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, CreateAccountResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CreateAccountHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<CreateAccountResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)
            {
                throw new Exception("User exists");
            }
            
            var user = new User()
            {
                Email = request.Email,
                Id = Guid.NewGuid(),
                UserName = request.Username,
                NormalizedEmail = request.Email,
                NormalizedUserName = request.Username,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Errors.Any())
            {
                await _userManager.AddToRoleAsync(user, AuthRoles.DefaultRole);
                await _signInManager.SignInAsync(user, false);
                // raise UserCreated
                return new CreateAccountResult();
            }
            
            var errors = result.Errors.First();
            throw new Exception(errors.Description);
        }
    }
}