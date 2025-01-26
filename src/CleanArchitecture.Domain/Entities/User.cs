namespace CleanArchitecture.Domain.Entities;

public class User
{
	public User()
	{
	}

	public User(string email, string password, string firstName, string lastName, long roleId)
	{
		Id = Guid.NewGuid().ToString();
		Email = email;
		var salt = HashingExtension.GenerateSalt();
		Salt = salt;
		Password = password.ComputeSha256HashWithSalt(salt);
		FirstName = firstName;
		LastName = lastName;
		RoleId = roleId;
	}

	public string Id { get; private set; }
	public string Email { get; private set; }
	public string Password { get; private set; }
	public string Salt { get; private set; }
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string FullName => $"{FirstName} {LastName}";

	public string? RefreshToken { get; private set; }
	public DateTime? RefreshTokenExpiryTime { get; private set; }

	public long RoleId { get; private set; }
	public Role Role { get; private set; }

	public bool VerifyPassword(string password)
	{
		return Password == password.ComputeSha256HashWithSalt(Salt);
	}

	public void SetRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
	{
		RefreshToken = refreshToken;
		RefreshTokenExpiryTime = refreshTokenExpiryTime;
	}
}