using CleanArchitecture.API.Extensions;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Authentication.Commands.Login;
using CleanArchitecture.Application.Features.Authentication.Commands.RefreshToken;
using CleanArchitecture.Application.Features.Authentication.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Endpoints;

public class Auth : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		const string pathname = "auth";

		app.MapGroup(this, pathname)
			.MapPost<TokenDto>(Login, "login")
			.MapPost<TokenDto>(RefreshToken, "refresh-token");

		app.MapGroup(this, pathname)
			.RequireAuthorization()
			.MapGet<object>(GetCurrentUser, "current");
	}

	public async Task<IResult> RefreshToken(ISender sender, [FromBody] RefreshTokenCommand command,
		CancellationToken cancellationToken)
	{
		var result = await sender.Send(command, cancellationToken);
		return ApiResult.Ok(result);
	}

	public async Task<IResult> Login(ISender sender, [FromBody] LoginCommand command,
		CancellationToken cancellationToken)
	{
		var result = await sender.Send(command, cancellationToken);
		return ApiResult.Ok(result);
	}

	public async Task<IResult> GetCurrentUser(ISender sender, CancellationToken cancellationToken)
	{
		var result = await sender.Send(new GetCurrentUserQuery(), cancellationToken);
		return ApiResult.Ok(result);
	}
}