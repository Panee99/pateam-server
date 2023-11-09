using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CourseConfigurations : AuditableEntityConfiguration<Course>
    {
        public override void Configure(EntityTypeBuilder<Course> builder)
        {
            base.Configure(builder);

            builder.HasMany(x => x.Feedback).WithOne(x => x.Course).HasForeignKey("CourseId");
        }
    }
}