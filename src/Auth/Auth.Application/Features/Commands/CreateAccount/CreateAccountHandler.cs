using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Events;
using Auth.Application.Exceptions;
using Auth.Domain;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StormShop.Common;
using StormShop.Common.Bus;

namespace Auth.Application.Features.Commands.CreateAccount
{
    [UsedImplicitly]
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, CreateAccountResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IBusProducer<AccountCreated> _busProducer;

        public CreateAccountHandler(UserManager<User> userManager, IBusProducer<AccountCreated> busProducer)
        {
            _userManager = userManager;
            _busProducer = busProducer;
        }

        public async Task<CreateAccountResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)
            {
                throw new AccountCreateExceptions("User exists");
            }

            var user = new User()
            {
                Email = request.Email,
                UserName = request.Username,
                NormalizedEmail = _userManager.NormalizeEmail(request.Email),
                NormalizedUserName = _userManager.NormalizeName(request.Username)
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Errors.Any())
            {
                var errors = result.Errors.First();
                throw new AccountCreateExceptions(errors.Description);
            }

            await _userManager.AddToRoleAsync(user, AuthRoles.DefaultRole);
            await _busProducer.Publish(user.Id.ToString("N"), new AccountCreated
            {
                AccountId = user.Id
            }, cancellationToken: cancellationToken);

            return new CreateAccountResult(user.Id);
        }
    }
}
