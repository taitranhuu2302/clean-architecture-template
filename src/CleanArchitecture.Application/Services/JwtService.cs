using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Application.Services;

public class JwtService : IJwtService
{
	private readonly JwtSetting _jwtSetting;
	private readonly byte[] _secretKey;

	public JwtService(IOptions<AppSetting> appSetting)
	{
		_jwtSetting = appSetting.Value.JwtSetting;
		_secretKey = Encoding.UTF8.GetBytes(_jwtSetting.Secret);
	}

	public async Task<TokenDto> GenerateTokenAsync(string userId, string email, string role)
	{
		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(ClaimTypes.NameIdentifier, userId),
			new Claim(JwtRegisteredClaimNames.Email, email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		claims.AddRange(new Claim(ClaimTypes.Role, role));

		var accessToken = GenerateAccessToken(claims);
		var refreshToken = GenerateRefreshToken();

		return new TokenDto(new JwtSecurityTokenHandler().WriteToken(accessToken), accessToken.ValidTo, refreshToken,
			DateTime.UtcNow.AddDays(_jwtSetting.RefreshTokenExpirationInDays));
	}


	public ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		try
		{
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = _jwtSetting.Issuer,
				ValidateAudience = true,
				ValidAudience = _jwtSetting.Audience,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
				ValidateLifetime = validateLifetime,
				ClockSkew = TimeSpan.Zero
			};

			return tokenHandler.ValidateToken(token, validationParameters, out _);
		}
		catch
		{
			return null;
		}
	}

	private JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
	{
		return new JwtSecurityToken(
			issuer: _jwtSetting.Issuer,
			audience: _jwtSetting.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwtSetting.AccessTokenExpirationInMinutes),
			signingCredentials: new SigningCredentials(
				new SymmetricSecurityKey(_secretKey),
				SecurityAlgorithms.HmacSha256)
		);
	}

	private static string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}
}