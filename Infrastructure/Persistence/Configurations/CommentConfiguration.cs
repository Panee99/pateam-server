using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration : AuditableEntityConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey("ParentId")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}