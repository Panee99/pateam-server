namespace Domain.Common
{
    public class Comment : AuditableEntity
    {
        public string Content { get; set; } = string.Empty;
        public int VoteUp { get; set; }
        public bool Hidden { get; set; }
        public Comment Parent { get; set; } = null!;
        public ICollection<Comment> Children { get; set; } = new List<Comment>();
    }
}