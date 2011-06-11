/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

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
