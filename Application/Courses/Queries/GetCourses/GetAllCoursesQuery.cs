using Application.Common.Interfaces;
using Application.Courses.Models;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries.GetCourses
{
    public class GetAllCoursesQuery : IRequest<List<CourseViewModel>>, IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<Course, CourseViewModel>();
        }
    }

    public class GetCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseViewModel>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetCoursesQueryHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<List<CourseViewModel>> Handle(GetAllCoursesQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Courses.Where(x => x.CreatedBy != null && x.CreatedBy.Id == _currentUser.UserId)
                .Select(x => x.Adapt<CourseViewModel>()).ToListAsync(cancellationToken);
        }
    }
}