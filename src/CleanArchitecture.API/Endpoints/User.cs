using CleanArchitecture.API.Extensions;
using CleanArchitecture.Application.Features.Users.Queries.GetCurrentUser;
using MediatR;

namespace CleanArchitecture.API.Endpoints;

public class User : EndpointGroupBase
{
	public override void Map(WebApplication app)
	{
		app.MapGroup(this, "users")
			.MapGet<object>(GetCurrentUser, "current");
	}

	public async Task<IResult> GetCurrentUser(ISender sender, CancellationToken cancellationToken)
	{
		await sender.Send(new GetCurrentUserQuery(), cancellationToken);
		return ApiResult.Ok(true);
	}
}