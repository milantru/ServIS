using System.Security.Cryptography;

namespace ServISWebApp.Auth
{
	// slightly edited, original is from: https://medium.com/dealeron-dev/storing-passwords-in-net-core-3de29a3da4d2
	public static class PasswordHasher
	{
		private static int saltSize = 16; // 128 bit
		private static int keySize = 32; // 256 bit
		private static int iterations = 10_000;

		public static bool AreCredentialsCorrect(string storedPasswordHash, string providedPassword)
		{
			bool isPasswordCorrect = false;
			bool needsUpgrade = true;
			while (needsUpgrade)
			{
				(isPasswordCorrect, needsUpgrade) = PasswordHasher.Check(storedPasswordHash, providedPassword);
			}

			return isPasswordCorrect;
		}

		private static (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
		{
			var parts = hash.Split('.', 3);

			if (parts.Length != 3)
			{
				//throw new FormatException("Unexpected hash format, should be formatted as `{iterations}.{salt}.{hash}`");
				return (false, false);
			}

			var iterations = Convert.ToInt32(parts[0]);
			var salt = Convert.FromBase64String(parts[1]);
			var key = Convert.FromBase64String(parts[2]);

			var needsUpgrade = iterations != PasswordHasher.iterations;

			using var algorithm = new Rfc2898DeriveBytes(
			  password,
			  salt,
			  iterations,
			  HashAlgorithmName.SHA256
			 );

			var keyToCheck = algorithm.GetBytes(keySize);

			var verified = keyToCheck.SequenceEqual(key);

			return (verified, needsUpgrade);
		}

		public static string Hash(string password)
		{
			using var algorithm = new Rfc2898DeriveBytes(
				password,
				saltSize,
				iterations,
				HashAlgorithmName.SHA256
			);

			var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
			var salt = Convert.ToBase64String(algorithm.Salt);

			return $"{iterations}.{salt}.{key}";
		}
	}
}
