namespace CleanArchitecture.Application.Options;

public class JwtSetting
{
	public string Secret { get; set; } = string.Empty;
	public string Issuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
	public int AccessTokenExpirationInMinutes { get; set; }
	public int RefreshTokenExpirationInDays { get; set; }
}