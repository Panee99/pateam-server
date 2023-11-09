namespace Domain.Common
{
    public abstract class Attachment : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}