namespace CleanArchitecture.Application.Common.Models;

public class TokenDto
{
	public TokenDto(string accessToken, DateTime? accessTokenExpiresIn, string refreshToken, DateTime? refreshTokenExpiresIn)
	{
		AccessToken = accessToken;
		AccessTokenExpiresIn = accessTokenExpiresIn;
		RefreshToken = refreshToken;
		RefreshTokenExpiresIn = refreshTokenExpiresIn;
	}
	public string AccessToken { get; set; }
	public DateTime? AccessTokenExpiresIn { get; set; }
	public string RefreshToken { get; set; }
	public DateTime? RefreshTokenExpiresIn { get; set; }
}