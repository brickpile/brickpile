using System;
using System.Text;
using System.Security.Cryptography;

namespace BrickPile.UI.Web.Security {
	public static class PasswordUtil {
        /// <summary>
        /// Creates the random salt.
        /// </summary>
        /// <returns></returns>
		public static string CreateRandomSalt() {
			var saltBytes = new Byte[4];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(saltBytes);
			return Convert.ToBase64String(saltBytes);
		}
        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
		public static string HashPassword(string pass, string salt) {
			byte[] bytes = Encoding.Unicode.GetBytes(pass);
			byte[] src = Encoding.Unicode.GetBytes(salt);
			byte[] dst = new byte[src.Length + bytes.Length];
			Buffer.BlockCopy(src, 0, dst, 0, src.Length);
			Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
			HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
			byte[] inArray = algorithm.ComputeHash(dst);
			return Convert.ToBase64String(inArray);
		}
	}
}
