using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        private readonly IAppDbContext _context;

        public CreateCourseCommandValidator(IAppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title).NotEmpty().MaximumLength(200).MustAsync(BeUniqueTitle);
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.Courses.AllAsync(x => x.Title != title, cancellationToken);
        }
    }
}