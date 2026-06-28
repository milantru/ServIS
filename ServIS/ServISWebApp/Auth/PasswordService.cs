using Microsoft.AspNetCore.Identity;
using ServISData.Models;

namespace ServISWebApp.Auth;

public sealed class PasswordService(IPasswordHasher<User> passwordHasher)
{
	public string Hash(User user, string password)
	{
		return passwordHasher.HashPassword(user, password);
	}

	public PasswordVerificationResult Verify(User user, string hashedPassword, string providedPassword)
	{
		return passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
	}
}
