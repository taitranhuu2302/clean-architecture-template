using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace CleanArchitecture.API.Extensions;

public static class SwaggerExtension
{
	public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
	{
		services.AddOpenApiDocument(config =>
		{
			config.Title = "Dental API";
			config.Version = "v1";
			config.Description = "Dental Practic API";

			config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
			{
				Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
				Name = "Authorization",
				In = NSwag.OpenApiSecurityApiKeyLocation.Header,
				Description = "Enter 'Bearer {your JWT token}' to access this API",

			});
			config.OperationProcessors.Add(
				new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
			config.OperationProcessors.Add(new MetadataOperationProcessor());
		});
		return services;
	}

	public static WebApplication UseCustomSwagger(this WebApplication app)
	{
		app.UseOpenApi();
		app.UseSwaggerUi();

		return app;
	}
}

public class MetadataOperationProcessor : IOperationProcessor
{
	public bool Process(OperationProcessorContext context)
	{
		// Get endpoint metadata
		var endpointMetadata = context.MethodInfo.GetCustomAttributes();

		// Process SwaggerOperationAttribute
		var attributes = endpointMetadata as Attribute[] ?? endpointMetadata.ToArray();
		var swaggerOperation = attributes.OfType<OpenApiOperationAttribute>().FirstOrDefault();
		if (swaggerOperation != null)
		{
			context.OperationDescription.Operation.Summary = swaggerOperation.Summary;
			context.OperationDescription.Operation.Description = swaggerOperation.Description;
		}

		// Process ProducesResponseTypeAttribute
		var producesResponseTypes = attributes.OfType<ProducesResponseTypeAttribute>();
		foreach (var responseType in producesResponseTypes)
		{
			var statusCode = responseType.StatusCode.ToString();
			var response = new NSwag.OpenApiResponse
			{
				Description = $"HTTP {statusCode} response"
			};

			response.Content.Add("application/json", new NSwag.OpenApiMediaType
			{
				Schema = context.SchemaGenerator.Generate(responseType.Type, context.SchemaResolver)
			});

			context.OperationDescription.Operation.Responses[statusCode] = response;
		}

		return true;
	}
}

