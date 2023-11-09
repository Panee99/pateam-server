using FluentValidation;

namespace Application.RootUsers.Commands.UpdateRootUser
{
    public class UpdateRootUserCommandValidator : AbstractValidator<UpdateRootUserCommand>
    {
        public UpdateRootUserCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Dob).NotEmpty();
        }
    }
}