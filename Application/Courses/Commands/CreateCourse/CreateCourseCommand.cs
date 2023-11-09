using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Courses.Commands.CreateCourse
{
    [Authorize(Roles = "Master,Admin,User")]
    public class CreateCourseCommand : IRequest<Guid>, IRegister
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CreateCourseCommand, Course>();
        }
    }

    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Guid>
    {
        private readonly IAppDbContext _context;

        public CreateCourseCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Adapt<Course>();

            _context.Courses.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}