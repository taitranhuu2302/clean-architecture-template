using System.Security.Claims;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IJwtService
{
	Task<TokenDto> GenerateTokenAsync(string userId, string email, string role);
	ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true);
}