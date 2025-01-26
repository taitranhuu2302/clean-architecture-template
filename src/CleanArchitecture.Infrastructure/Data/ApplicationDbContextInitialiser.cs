using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Data;

public static class InitialiserExtensions
{
	public static async Task InitialiseDatabaseAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

		await initialiser.InitialiseAsync();

		await initialiser.SeedAsync();
	}
}

public class ApplicationDbContextInitialiser
{
	private readonly ILogger<ApplicationDbContextInitialiser> _logger;
	private readonly ApplicationDbContext _context;
	private readonly IConfiguration _configuration;
	private readonly IMemoryCache _cache;

	public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context
		, IConfiguration configuration, IMemoryCache cache)
	{
		_logger = logger;
		_context = context;
		_configuration = configuration;
		_cache = cache;
	}

	public async Task InitialiseAsync()
	{
		try
		{
			await _context.Database.MigrateAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initialising the database");
			throw;
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while seeding the database");
			throw;
		}
	}

	private async Task TrySeedAsync()
	{
		await _context.SeedRoles();
		await _context.SeedUsers();
	}
}