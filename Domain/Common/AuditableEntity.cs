using Domain.Entities;

namespace Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public RootUser? CreatedBy { get; set; }
        public RootUser? UpdatedBy { get; set; }
        public RootUser? DeletedBy { get; set; }
    }
}