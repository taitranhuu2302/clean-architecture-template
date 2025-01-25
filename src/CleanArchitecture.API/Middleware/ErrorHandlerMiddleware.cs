using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Common;
using FluentValidation;

namespace CleanArchitecture.API.Middleware;

public class ErrorHandlerMiddleware
{
	private readonly RequestDelegate _next;

	public ErrorHandlerMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context, ILogger<ErrorHandlerMiddleware> logger)
	{
		context.Response.ContentType = "application/json";
		try
		{
			await _next(context);
		}
		catch (UnauthorizedAccessException)
		{
			var result =
				Result<object>.Fail(
					"Authentication is required to access this resource.");
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;

			await context.Response.WriteAsJsonAsync(result);
		}
		catch (ForbiddenAccessException)
		{
			var result =
				Result<object>.Fail(
					"Access to the requested resource is forbidden.");
			context.Response.StatusCode = StatusCodes.Status403Forbidden;

			await context.Response.WriteAsJsonAsync(result);
		}
		catch (NotFoundException ex)
		{
			var result = Result<object>.Fail(ex.Message);
			context.Response.StatusCode = StatusCodes.Status404NotFound;

			await context.Response.WriteAsJsonAsync(result);
		}
		catch (ValidationException ex)
		{
			var result = Result<object>.Fail(string.Join("|", ex.Errors.Select(e => e.ErrorMessage)));
			context.Response.StatusCode = StatusCodes.Status400BadRequest;

			await context.Response.WriteAsJsonAsync(result);
		}
		catch (BadHttpRequestException ex)
		{
			var result = Result<object>.Fail(ex.Message);
			context.Response.StatusCode = StatusCodes.Status400BadRequest;

			await context.Response.WriteAsJsonAsync(result);
		}
		catch (ArgumentException ex)
		{
			var result = Result<object>.Fail(ex.Message);
			context.Response.StatusCode = StatusCodes.Status400BadRequest;

			await context.Response.WriteAsJsonAsync(result);
		}
		catch (TimeoutException)
		{
			var result = Result<object>.Fail("The request timed out.");
			context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
			await context.Response.WriteAsJsonAsync(result);
		}
		catch (HttpRequestException)
		{
			var result = Result<object>.Fail("A network error occurred.");
			context.Response.StatusCode = StatusCodes.Status502BadGateway;
			await context.Response.WriteAsJsonAsync(result);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred: {Message}", ex.Message);
			var result =
				Result<object>.Fail(
					"Sorry, it looks like something went wrong on our end. Please try again in a few minutes.");
			context.Response.StatusCode = StatusCodes.Status400BadRequest;

			await context.Response.WriteAsJsonAsync(result);
		}
	}
}