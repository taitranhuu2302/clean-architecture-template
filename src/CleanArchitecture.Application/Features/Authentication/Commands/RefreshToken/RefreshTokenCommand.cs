using System.Security.Claims;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<TokenDto>
{
	public string AccessToken { get; set; } = string.Empty;
	public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenCommandHandler(IApplicationDbContext context, IJwtService jwtService)
	: IRequestHandler<RefreshTokenCommand, TokenDto>
{
	public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var principal = jwtService.ValidateToken(request.AccessToken, validateLifetime: false);

		if (principal == null)
		{
			throw new ArgumentException("Invalid token.");
		}

		var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

		var user = await context.Users
			.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

		if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
		{
			throw new ArgumentException("Invalid refresh token");
		}

		var token = await jwtService.GenerateTokenAsync(user.Id, user.Email, user.Role.Name);

		user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresIn!.Value);
		await context.SaveChangesAsync(cancellationToken);

		return token;
	}
}

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
	public RefreshTokenCommandValidator()
	{
		RuleFor(p => p.AccessToken)
			.NotEmpty().WithMessage("{PropertyName} is required.");

		RuleFor(p => p.RefreshToken)
			.NotEmpty().WithMessage("{PropertyName} is required.");
	}
}