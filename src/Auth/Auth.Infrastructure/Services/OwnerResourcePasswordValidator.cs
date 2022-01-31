using System.Threading.Tasks;
using Auth.Domain;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Services
{
    public class OwnerResourcePasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public OwnerResourcePasswordValidator(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);

            if (user is null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
                return;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, context.Password, false, true);

            if (!signInResult.Succeeded)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
                return;
            }

            context.Result = new GrantValidationResult(
                user.Id.ToString(),
                "Bearer");
        }
    }
}