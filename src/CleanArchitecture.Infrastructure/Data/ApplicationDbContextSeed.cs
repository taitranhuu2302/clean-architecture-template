using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data;

public static class ApplicationDbContextSeed
{
	public static async Task SeedRoles(this ApplicationDbContext context)
	{
		var isExist = await context.Roles.AnyAsync();

		if (isExist) return;

		string[] roleNames = ["Admin", "User", "Guest"];

		foreach (var roleName in roleNames)
		{
			await context.Roles.AddAsync(new Role(roleName, roleName));
		}

		await context.SaveChangesAsync();
	}

	public static async Task SeedUsers(this ApplicationDbContext context)
	{
		var isExist = await context.Users.AnyAsync();

		if (isExist) return;

		var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

		if (adminRole == null) return;

		var adminUser = new User("admin@localhost.com", "Test!123", "Admin", "System", adminRole.Id);

		await context.Users.AddAsync(adminUser);
		await context.SaveChangesAsync();
	}
}