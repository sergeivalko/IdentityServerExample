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

#pragma warning disable CA2000

namespace Auth.Tests.Features
{
    public class CreateAccountTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly Mock<IBusProducer<AccountCreated>> _busMock;

        public CreateAccountTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _busMock = new Mock<IBusProducer<AccountCreated>>();
        }

        [Fact]
        public async Task HaveDuplicateShouldThrowException()
        {
            var dbContext = _fixture.CreateStore();
            await dbContext.Users.AddAsync(new User
            {
                UserName = "sergei",
                Email = "example1@gmail.com",
                NormalizedEmail = "example1@gmail.com".ToUpperInvariant()
            });
            await dbContext.SaveChangesAsync();

            var user = new CreateAccountCommand("sergei", "example1@gmail.com", "xxxxxDDDhhhaswWWd");

            using var userStore = new UserStore<User, Role, AuthContext, Guid>(dbContext);
            using var accountManagerMock = new UserManager<User>(userStore, null, new PasswordHasher<User>(),
                new List<IUserValidator<User>> { new UserValidator<User>(new IdentityErrorDescriber()) },
                new List<IPasswordValidator<User>> { new PasswordValidator<User>(new IdentityErrorDescriber()) },
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null, new Mock<ILogger<UserManager<User>>>().Object);
            var handler = new CreateAccountHandler(accountManagerMock, _busMock.Object);

            var exception = await Assert.ThrowsAsync<AccountCreateExceptions>(() =>
                handler.Handle(user, CancellationToken.None));

            Assert.Contains("User exists", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public async Task UserValidShouldSuccessfullyCreate()
        {
            var user = new CreateAccountCommand("sergei", "example@gmail.com", "testPassword1#");
            var dbContext = _fixture.CreateStore();

            using var userStore = new UserStore<User, Role, AuthContext, Guid>(dbContext);
            using var accountManagerMock = new UserManager<User>(userStore, null, new PasswordHasher<User>(),
                new List<IUserValidator<User>> { new UserValidator<User>(new IdentityErrorDescriber()) },
                new List<IPasswordValidator<User>> { new PasswordValidator<User>(new IdentityErrorDescriber()) },
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null, new Mock<ILogger<UserManager<User>>>().Object);
            var handler = new CreateAccountHandler(accountManagerMock, _busMock.Object);
            
            var result = await handler.Handle(user, CancellationToken.None);

            Assert.NotEqual(result.AccountId, Guid.Empty);
            _busMock.Verify(x => x.Publish(It.IsAny<string>(),
                    It.Is<AccountCreated>(user1 =>
                        user1.AccountId == result.AccountId
                    ),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
        }
    }
}
