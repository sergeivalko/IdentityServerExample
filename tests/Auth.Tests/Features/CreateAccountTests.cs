using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Events;
using Auth.Application.Exceptions;
using Auth.Application.Features.Commands.CreateAccount;
using Auth.Domain;
using Auth.Infrastructure.Context;
using Auth.Tests.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StormShop.Common.Bus;
using Xunit;

namespace Auth.Tests.Features
{
    public class CreateAccountTests : IClassFixture<DatabaseFixture>
    {
        private readonly CreateAccountHandler _handler;
        private readonly Mock<IBusProducer<UserCreated>> _busMock;
        private readonly AuthContext _dbContext;

        public CreateAccountTests(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.CreateStore();
            IUserStore<User> userStore = new UserStore<User, Role, AuthContext, Guid>(_dbContext);

            var accountManagerMock = new UserManager<User>(userStore, null, new PasswordHasher<User>(),
                new List<IUserValidator<User>>
                {
                    new UserValidator<User>(new IdentityErrorDescriber())
                }, new List<IPasswordValidator<User>>
                {
                    new PasswordValidator<User>(new IdentityErrorDescriber())
                }, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null, new Mock<ILogger<UserManager<User>>>().Object);
            _busMock = new Mock<IBusProducer<UserCreated>>();
            _handler = new CreateAccountHandler(accountManagerMock, _busMock.Object);
        }

        [Fact]
        public async Task HaveDuplicate_ShouldThrowException()
        {
            await _dbContext.Users.AddAsync(new User
            {
                UserName = "sergei",
                Email = "example1@gmail.com",
                NormalizedEmail = "example1@gmail.com".ToUpperInvariant()
            });
            await _dbContext.SaveChangesAsync();

            var user = new CreateAccountCommand("sergei", "example1@gmail.com", "xxxxxDDDhhhaswWWd");

            var exception = await Assert.ThrowsAsync<AccountCreateExceptions>(() =>
                _handler.Handle(user, CancellationToken.None));

            Assert.Contains("User exists", exception.Message);
        }
        
        [Fact]
        public async Task UserValid_ShouldSuccessfullyCreate()
        {
            var user = new CreateAccountCommand("sergei", "example@gmail.com", "testPassword1#");

            var result = await _handler.Handle(user, CancellationToken.None);

            Assert.NotEqual(result.AccountId, Guid.Empty);
            _busMock.Verify(x => x.Publish(It.IsAny<string>(),
                    It.Is<UserCreated>(user1 =>
                        user1.AccountId == result.AccountId
                    ),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
        }
    }
}