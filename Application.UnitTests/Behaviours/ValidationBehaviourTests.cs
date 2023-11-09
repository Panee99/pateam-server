using Application.Common.Behaviours;
using Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.UnitTests.Behaviours
{
    public class ValidationBehaviourTests
    {
        [Test]
        public void Validator_Should_Throw_BadRequestException_When_Pass_Invalid_Request_Params()
        {
            // ARRANGE
            var command = new TestCommand(); // create command without `Title`
            var commandHandler = new TestCommandHandler();
            var validator = new TestCommandValidator();
            var validationBehaviour =
                new ValidationBehaviour<TestCommand, Guid>(new List<IValidator<TestCommand>> { validator });

            // ASSERT
            Assert.ThrowsAsync<BadRequestException>(() => validationBehaviour.Handle(command,
                () => commandHandler.Handle(command, CancellationToken.None),
                CancellationToken.None));
        }

        [Test]
        public void Validator_Should_Success_When_Pass_Valid_Request_Params()
        {
            // ARRANGE
            var command = new TestCommand { Title = "Title" }; // create a valid command
            var commandHandler = new TestCommandHandler();
            var validator = new TestCommandValidator();
            var validationBehaviour =
                new ValidationBehaviour<TestCommand, Guid>(new List<IValidator<TestCommand>> { validator });

            // ASSERT
            Assert.DoesNotThrowAsync(() => validationBehaviour.Handle(command,
                () => commandHandler.Handle(command, CancellationToken.None), CancellationToken.None));
        }

        private class TestCommand : IRequest<Guid>
        {
            public string Title { get; set; } = string.Empty;
        }

        private class TestCommandHandler : IRequestHandler<TestCommand, Guid>
        {
            public Task<Guid> Handle(TestCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Guid.NewGuid());
            }
        }

        private class TestCommandValidator : AbstractValidator<TestCommand>
        {
            public TestCommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            }
        }
    }
}