using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Authentication.Commands.Login;

public class LoginCommand : IRequest<TokenDto>
{
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}

public class LoginCommandHandler (IApplicationDbContext context, IJwtService jwtService): IRequestHandler<LoginCommand, TokenDto>
{
	public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await context.Users
			.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

		if (user == null || !user.VerifyPassword(request.Password))
		{
			throw new ArgumentException("Email or password is incorrect.");
		}

		var token = await jwtService.GenerateTokenAsync(user.Id, user.Email, user.Role.Name);

		user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresIn!.Value);
		await context.SaveChangesAsync(cancellationToken);

		return token;
	}
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(p => p.Email)
			.NotEmpty().WithMessage("{PropertyName} is required.")
			.EmailAddress().WithMessage("A valid email is required.");

		RuleFor(p => p.Password)
			.NotEmpty().WithMessage("{PropertyName} is required.");
	}
}