using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Domain.Extensions;

public static class HashingExtension
{
	public static string ComputeSha256Hash(this string rawData)
	{
		// Create a SHA256
		using (SHA256 sha256Hash = SHA256.Create())
		{
			// ComputeHash - returns byte array
			byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

			// Convert byte array to a string
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				builder.Append(bytes[i].ToString("x2"));
			}
			return builder.ToString();
		}
	}

	public static string ComputeSha256HashWithSalt(this string rawData, string salt)
	{
		using (SHA256 sha256 = SHA256.Create())
		{
			byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(rawData + salt);
			byte[] hashedBytes = sha256.ComputeHash(saltedPasswordBytes);

			StringBuilder builder = new StringBuilder();
			foreach (byte b in hashedBytes)
			{
				builder.Append(b.ToString("x2"));
			}
			return builder.ToString();
		}
	}
	public static string GenerateSalt()
	{
		// Create a SHA256
		byte[] tokenBytes = new byte[16];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(tokenBytes);
		}
		string token = BitConverter.ToString(tokenBytes).Replace("-", "");
		return token;
	}
}
