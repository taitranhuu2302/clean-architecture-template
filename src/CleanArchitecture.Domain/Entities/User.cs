namespace CleanArchitecture.Domain.Entities;

public class User
{
	public User()
	{
	}

	public User(string email, string password, string firstName, string lastName)
	{
		Id = Guid.NewGuid().ToString();
		Email = email;
		var salt = HashingExtension.GenerateSalt();
		Salt = salt;
		Password = password.ComputeSha256HashWithSalt(salt);
		FirstName = firstName;
		LastName = lastName;
	}

	public string Id { get; private set; }
	public string Email { get; private set; }
	public string Password { get; private set; }
	public string Salt { get; private set; }
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string FullName => $"{FirstName} {LastName}";

	public bool VerifyPassword(string password)
	{
		return Password == password.ComputeSha256HashWithSalt(Salt);
	}
}