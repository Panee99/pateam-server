using Domain.Common;
using Mapster;

namespace Application.Common.Models
{
    public abstract class AuditableViewModel 
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public RootUserDto? CreatedBy { get; set; }
        public RootUserDto? UpdatedBy { get; set; }
        public RootUserDto? DeletedBy { get; set; }
    }
}