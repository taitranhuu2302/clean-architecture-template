using Ardalis.GuardClauses;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Extensions;

namespace CleanArchitecture.API.Extensions;

public static class EndpointRouteBuilderExtensions
	{
		public static IEndpointRouteBuilder MapGet<T>(this IEndpointRouteBuilder builder, Delegate handler,
			string pattern = "", string? summary = null, string? description = null)
		{
			Guard.Against.AnonymousMethod(handler);

			builder.MapGet(pattern, handler)
				.WithName(handler.Method.Name)
				.AddMetaData<T>(summary, description);

			return builder;
		}

		public static IEndpointRouteBuilder MapPost<T>(this IEndpointRouteBuilder builder, Delegate handler,
			string pattern = "", string? summary = null, string? description = null)
		{
			Guard.Against.AnonymousMethod(handler);

			builder.MapPost(pattern, handler)
				.WithName(handler.Method.Name)
				.AddMetaData<T>(summary, description);

			return builder;
		}

		public static IEndpointRouteBuilder MapPut<T>(this IEndpointRouteBuilder builder, Delegate handler,
			string pattern = "", string? summary = null, string? description = null)
		{
			Guard.Against.AnonymousMethod(handler);

			builder.MapPut(pattern, handler)
				.WithName(handler.Method.Name)
				.AddMetaData<T>(summary, description);

			return builder;
		}

		public static IEndpointRouteBuilder MapDelete<T>(this IEndpointRouteBuilder builder, Delegate handler,
			string pattern = "", string? summary = null, string? description = null)
		{
			Guard.Against.AnonymousMethod(handler);

			builder.MapDelete(pattern, handler)
				.WithName(handler.Method.Name)
				.AddMetaData<T>(summary, description);

			return builder;
		}

		static RouteHandlerBuilder AddMetaData<T>(this RouteHandlerBuilder endpoint, string? summary = null,
			string? description = null)
		{
			endpoint.WithOpenApi(operation =>
			{
				operation.Summary = summary;
				operation.Description = description;
				return operation;
			});

			endpoint.Produces<Result<T>>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status204NoContent)
				.Produces<Result<object>>(StatusCodes.Status401Unauthorized)
				.Produces<Result<object>>(StatusCodes.Status403Forbidden)
				.Produces<Result<object>>(StatusCodes.Status400BadRequest)
				.Produces<Result<object>>(StatusCodes.Status503ServiceUnavailable)
				.Produces<Result<object>>(StatusCodes.Status502BadGateway)
				.Produces<Result<object>>(StatusCodes.Status500InternalServerError);

			return endpoint;
		}

		static Type? GetActualReturnType(Type type)
		{
			if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Task<>) ||
			                           type.GetGenericTypeDefinition() == typeof(ValueTask<>)))
			{
				return type.GetGenericArguments()[0];
			}

			if (type == typeof(Task) || type == typeof(ValueTask))
			{
				return typeof(void);
			}

			return type;
		}
	}