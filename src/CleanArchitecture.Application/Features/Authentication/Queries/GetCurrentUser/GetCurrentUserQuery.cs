using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Authentication.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserDto>
{
}

public class GetCurrentUserQueryHandler(IApplicationDbContext context, IMapper mapper, IUser currentUser)
	: IRequestHandler<GetCurrentUserQuery, UserDto>
{
	public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
	{
		var user = await context.Users.AsNoTracking()
			.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken);

		if (user == null)
		{
			throw new ArgumentException("User not found");
		}

		return mapper.Map<UserDto>(user);
	}
}