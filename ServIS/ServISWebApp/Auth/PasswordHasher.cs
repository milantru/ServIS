using System.Security.Cryptography;

namespace ServISWebApp.Auth
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords.
    /// <para>
    /// <remarks>
    /// This class is slightly edited, original is from: 
    /// <see href="https://medium.com/dealeron-dev/storing-passwords-in-net-core-3de29a3da4d2"/>
    /// </remarks>
    /// </para>
    /// </summary>
    public static class PasswordHasher
	{
		private static int saltSize = 16; // 128 bit
		private static int keySize = 32; // 256 bit
		private static int iterations = 10_000;

        /// <summary>
        /// Checks if the provided password matches the stored password hash.
        /// </summary>
        /// <param name="storedPasswordHash">The stored password hash.</param>
        /// <param name="providedPassword">The password provided for verification.</param>
        /// <returns>True if the provided password is correct; otherwise false.</returns>
        public static bool AreCredentialsCorrect(string storedPasswordHash, string providedPassword)
		{
			bool isPasswordCorrect = false;
			bool needsUpgrade = true;
			while (needsUpgrade)
			{
				(isPasswordCorrect, needsUpgrade) = Check(storedPasswordHash, providedPassword);
			}

			return isPasswordCorrect;
		}

        /// <summary>
        /// Computes the hash for the specified password.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password string.</returns>
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

        /// <summary>
        /// Checks if the provided password matches the provided hash.
        /// </summary>
        /// <param name="hash">The password hash in the format "{iterations}.{salt}.{hash}".</param>
        /// <param name="password">The password provided for verification.</param>
        /// <returns>A tuple containing the verification result and whether the hash needs to be upgraded.</returns>
        private static (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                // Unexpected hash format, should be formatted as "{iterations}.{salt}.{hash}"
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
    }
}
