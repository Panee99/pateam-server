using Domain.Common;

namespace Domain.Entities
{
    public class Course : AuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public ICollection<CourseFeedback> Feedback { get; set; } = new List<CourseFeedback>();
    }
}