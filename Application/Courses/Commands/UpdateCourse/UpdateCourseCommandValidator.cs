using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        private readonly IAppDbContext _context;

        public UpdateCourseCommandValidator(IAppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must be not exceed 200 characters.")
                .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.Courses.AllAsync(x => x.Title != title, cancellationToken);
        }
    }
}