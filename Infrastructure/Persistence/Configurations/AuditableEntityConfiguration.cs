using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AuditableEntityConfiguration<TEntity> : BaseEntityConfigurations<TEntity>
        where TEntity : AuditableEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey("CreatedById");
            builder.HasOne(x => x.UpdatedBy).WithMany().HasForeignKey("UpdatedById");
            builder.HasOne(x => x.DeletedBy).WithMany().HasForeignKey("DeletedById");

            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}