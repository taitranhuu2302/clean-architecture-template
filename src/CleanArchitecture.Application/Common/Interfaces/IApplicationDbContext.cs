using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }

	IDbContextTransaction BeginTransaction();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}