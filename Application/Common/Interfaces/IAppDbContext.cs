using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Attachment> Attachments { get; }
        DbSet<Comment> Comments { get; }
        DbSet<Feedback> Feedback { get; }
        DbSet<Group> Groups { get; }
        DbSet<Tag> Tags { get; }
        DbSet<Course> Courses { get; }
        DbSet<CourseFeedback> CourseFeedback { get; }
        DbSet<RootUser> RootUsers { get; }
        DbSet<UserAvatar> UserAvatars { get; }
        
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}