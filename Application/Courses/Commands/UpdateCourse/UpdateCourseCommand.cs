using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommand : IRequest, IRegister
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateCourseCommand, Course>().IgnoreNullValues(true);
        }
    }

    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
    {
        private readonly IAppDbContext _context;

        public UpdateCourseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Courses.FindAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Course), request.Id);
            }

            var updatedEntity = entity.Adapt<Course>();
            _context.Courses.Update(updatedEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}