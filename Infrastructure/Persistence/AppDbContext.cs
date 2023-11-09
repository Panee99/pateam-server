using System.Reflection;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.Behaviours;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ICurrentUserService currentUserService,
            IDomainEventService domainEventService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Feedback> Feedback => Set<Feedback>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<CourseFeedback> CourseFeedback => Set<CourseFeedback>();
        public DbSet<RootUser> RootUsers => Set<RootUser>();
        public DbSet<UserAvatar> UserAvatars => Set<UserAvatar>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor(_currentUserService, _dateTime));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private async Task DispatchEvents(IEnumerable<DomainEvent> events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await _domainEventService.Publish(@event);
            }
        }
    }
}