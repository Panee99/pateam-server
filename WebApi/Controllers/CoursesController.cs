using Application.Courses.Commands.CreateCourse;
using Application.Courses.Models;
using Application.Courses.Queries.GetCourses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CoursesController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateCourseCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseViewModel>>> GetAll()
        {
            return await Mediator.Send(new GetAllCoursesQuery());
        }
    }
}