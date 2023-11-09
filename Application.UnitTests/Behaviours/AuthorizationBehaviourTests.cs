using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using Moq;

namespace Application.UnitTests.Behaviours
{
    internal class AuthorizationBehaviourTests
    {
        private Mock<ICurrentUserService> _currentUserService = null!;
        private Mock<IIdentityService> _identityService = null!;

        [SetUp]
        public void SetUp()
        {
            _currentUserService = new Mock<ICurrentUserService>();
            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public void Authorization_Should_Throw_UnauthorizedException_When_User_Not_Login()
        {
            // ARRANGE
            _currentUserService.Setup(x => x.UserId).Returns(null as Guid?); // User does not login
            _identityService.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false); // The user does not have the required roles
            var command = new TestCommand();

            // ASSERT
            AssertCommandAuthorized(command);

            var commandHandler = new TestCommandHandler();

            var authorizationBehaviour =
                new AuthorizationBehaviour<TestCommand, Guid>(_currentUserService.Object, _identityService.Object);

            Assert.ThrowsAsync<UnauthorizedException>(async () => await authorizationBehaviour.Handle(command,
                () => commandHandler.Handle(command, CancellationToken.None), CancellationToken.None));
        }

        [Test]
        public void Authorization_Should_Throw_ForbiddenException_When_User_Do_Not_Have_Role_Permission()
        {
            // ARRANGE
            _currentUserService.Setup(x => x.UserId).Returns(Guid.NewGuid()); // User logged in
            _identityService.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false); // The user does not have the required roles
            var command = new TestCommand();

            // ASSERT
            AssertCommandAuthorized(command);

            var commandHandler = new TestCommandHandler();

            var authorizationBehaviour =
                new AuthorizationBehaviour<TestCommand, Guid>(_currentUserService.Object, _identityService.Object);

            Assert.ThrowsAsync<ForbiddenException>(() => authorizationBehaviour.Handle(command,
                () => commandHandler.Handle(command, CancellationToken.None), CancellationToken.None));
        }

        [Test]
        public void Authorization_Should_Success_On_Valid_User()
        {
            // ARRANGE
            _currentUserService.Setup(x => x.UserId)
                .Returns(It.IsAny<Guid>()); // User logged in
            _identityService.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(true); // User have a valid role
            var command = new TestCommand();

            // ASSERT
            AssertCommandAuthorized(command);

            var commandHandler = new TestCommandHandler();

            var authorizationBehaviour =
                new AuthorizationBehaviour<TestCommand, Guid>(_currentUserService.Object, _identityService.Object);

            Assert.That(
                () => authorizationBehaviour.Handle(command,
                    () => commandHandler.Handle(command, CancellationToken.None), CancellationToken.None),
                Is.InstanceOf<Guid>());
        }

        private static void AssertCommandAuthorized<TResponse>(IRequest<TResponse> command)
        {
            var authorizeAttributes = command.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();
            Assert.That(authorizeAttributes, Is.Not.Empty, $"{nameof(TestCommand)} missing [Authorize]");

            var roles = authorizeAttributes.Where(x => !string.IsNullOrWhiteSpace(x.Roles))
                .SelectMany(x => x.Roles.Split(","));
            Assert.That(roles, Is.Not.Empty, $"{nameof(TestCommand)} missing Authorize with roles");
        }

        [Authorize(Roles = "admin")]
        private class TestCommand : IRequest<Guid>
        {
        }

        private class TestCommandHandler : IRequestHandler<TestCommand, Guid>
        {
            public Task<Guid> Handle(TestCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Guid.NewGuid());
            }
        }
    }
}