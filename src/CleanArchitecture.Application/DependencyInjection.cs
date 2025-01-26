using System.Reflection;
using System.Text;
using CleanArchitecture.Application.Common.Behaviors;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Options;
using CleanArchitecture.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<AppSetting>(configuration);

		var config = configuration.Get<AppSetting>();

		services.AddScoped<IJwtService, JwtService>();
		services.AddAuthentication(opt =>
		{
			opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(opt =>
		{
			opt.TokenValidationParameters = new TokenValidationParameters
			{
				// Validate issuer, audience, lifetime, và signing key
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = config!.JwtSetting.Issuer,
				ValidAudience = config!.JwtSetting.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(config!.JwtSetting.Secret)
				),

				ClockSkew = TimeSpan.Zero
			};

			opt.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
				{
					if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
					{
						context.Response.Headers.Append("Token-Expired", "true");
					}
					return Task.CompletedTask;
				}
			};
		});


		services.AddAutoMapper(Assembly.GetExecutingAssembly());
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
		);

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}