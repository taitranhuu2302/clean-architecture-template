using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
	private readonly IUser _user;
	private readonly ILogger<AuditableEntityInterceptor> _logger;

	public AuditableEntityInterceptor(
		IUser user, ILogger<AuditableEntityInterceptor> logger)
	{
		_user = user;
		_logger = logger;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		try
		{
			UpdateEntities(eventData.Context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating entities");
			throw;
		}

		return base.SavingChanges(eventData, result);
	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
		InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		try
		{
			UpdateEntities(eventData.Context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating entities");
			throw;
		}

		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public void UpdateEntities(DbContext? context)
	{
		if (context == null) return;

		foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
		{
			if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
			{
				if (entry.State == EntityState.Added)
				{
					entry.Entity.UpdateCreated(_user.Id);
				}

				entry.Entity.UpdateLastModified(_user.Id);
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