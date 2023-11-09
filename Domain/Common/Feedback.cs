namespace Domain.Common
{
    public abstract class Feedback : AuditableEntity
    {
        public string Content { get; set; } = string.Empty;
        public int StarRating { get; set; }
    }
}