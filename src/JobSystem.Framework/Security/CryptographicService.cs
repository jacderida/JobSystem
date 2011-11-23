using System;
using System.Security.Cryptography;
using System.Text;

namespace JobSystem.Framework.Security
{
	/// <summary>
	/// Implementation of <see cref="ICryptographicService"/> that usese the SHA algorithm to calculate a hash. 
	/// </summary>
	/// <remarks>
	/// Original Source: http://davidhayden.com/blog/dave/archive/2004/02/16/157.aspx
	/// </remarks>
	public class CryptographicService : ICryptographicService
	{
		private const int SaltSize = 64;

		#region ICryptographicService Members

		/// <summary>
		/// Generate a salt for a password. 
		/// </summary>
		/// <returns>A base 64 encoded salt</returns>
		public string GenerateSalt()
		{
			var rng = new RNGCryptoServiceProvider();
			var buffer = new byte[SaltSize];
			rng.GetBytes(buffer);
			return Convert.ToBase64String(buffer);
		}

		/// <summary>
		/// Compute a hash for the given value
		/// </summary>
		/// <param name="value">The value to hash</param>
		/// <returns>The hashed value</returns>
		public string ComputeHash(string value)
		{
			var algorithm = SHA512.Create();
			byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
			return Convert.ToBase64String(hash);
		}

		/// <summary>
		/// Compute a hash of the password and given salt
		/// </summary>
		/// <param name="password">The password</param>
		/// <param name="salt">The salt to append to the password</param>
		/// <returns>The computed hash</returns>
		public string ComputeHash(string password, string salt)
		{
			return ComputeHash(password + salt);
		}

		#endregion
	}
}