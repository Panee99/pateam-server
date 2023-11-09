using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Behaviours
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public SoftDeleteInterceptor(ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            return SavingChangesHandler(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = new())
        {
            return ValueTask.FromResult(SavingChangesHandler(eventData, result));
        }

        private InterceptionResult<int> SavingChangesHandler(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null)
            {
                return result;
            }

            var auditableEntries = eventData.Context.ChangeTracker.Entries().Where(x => x is
                { Entity: AuditableEntity, State: EntityState.Added or EntityState.Modified or EntityState.Deleted });

            var currentUserId = _currentUserService.UserId;
            var now = _dateTime.Now;

            foreach (var entry in auditableEntries)
            {
                if (entry.Entity is AuditableEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedById").CurrentValue = currentUserId;
                        entity.CreatedAt = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property("UpdatedById").CurrentValue = currentUserId;
                        entity.UpdatedAt = now;
                    }
                    else
                    {
                        entry.Property("DeletedById").CurrentValue = currentUserId;
                        entity.DeletedAt = now;
                        entry.State = EntityState.Modified;
                    }
                }
            }

            return result;
        }
    }
}