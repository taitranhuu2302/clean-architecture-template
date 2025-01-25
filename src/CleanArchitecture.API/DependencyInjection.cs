using System.Text.Json;
using CleanArchitecture.API.Extensions;
using CleanArchitecture.API.Services;
using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API;

public static class DependencyInjection
{
	public static IServiceCollection AddWebApi(this IServiceCollection services)
	{
		services.AddControllers().AddJsonOptions(opt =>
		{
			opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		});

		services.AddCustomSwagger();

		services.AddScoped<IUser, CurrentUser>();
		services.AddHttpContextAccessor();
		services.AddHealthChecks();
		services.AddRazorPages();

		services.Configure<ApiBehaviorOptions>(options =>
			options.SuppressModelStateInvalidFilter = true);

		services.AddEndpointsApiExplorer();

		services.AddDistributedMemoryCache();

		return services;
	}
}