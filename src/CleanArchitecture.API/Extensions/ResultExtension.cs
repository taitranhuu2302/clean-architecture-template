using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.API.Extensions;

public static class ApiResult
{
	public static IResult Ok<T>(T data, string message = "Success")
	{
		return Results.Ok(Result<T>.Complete(data, message));
	}
}