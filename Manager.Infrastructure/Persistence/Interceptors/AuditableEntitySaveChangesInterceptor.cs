using Manager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Manager.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor(
    IUserManagerService currentUserService, IDateTime dateTime) : SaveChangesInterceptor
{
    public readonly IUserManagerService _currentUserService = currentUserService;
    private readonly IDateTime _dateTime = dateTime;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context!);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context!);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext context)
    {
        if (context is null) return;

        foreach(var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if(entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUserService.UserId;
                entry.Entity.Created = _dateTime.Now;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Property(p => p.CreatedBy).IsModified = false;
                entry.Property(p => p.Created).IsModified = false;
                entry.Entity.LastModifiedBy = _currentUserService.UserId;
                entry.Entity.LastModified = _dateTime.Now;
            }
        }
    }
}


public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}