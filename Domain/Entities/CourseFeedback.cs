using Domain.Common;

namespace Domain.Entities
{
    public class CourseFeedback : Feedback
    {
        public Course Course { get; set; } = null!;
    }
}