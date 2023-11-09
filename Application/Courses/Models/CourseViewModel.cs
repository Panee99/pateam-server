using Application.Common.Models;

namespace Application.Courses.Models
{
    public class CourseViewModel : AuditableViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }
    }
}