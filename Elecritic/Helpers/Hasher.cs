using System.Security.Cryptography;
using System.Text;

namespace Elecritic.Helpers {

    /// <summary>
    /// Helper class that provides a static method to hash a password.
    /// </summary>
    public static class Hasher {

        /// <summary>
        /// Gets the bytes from <paramref name="inputString"/> and applies SHA256 to get their hash.
        /// </summary>
        /// <returns>Hash of <paramref name="inputString"/> bytes.</returns>
        private static byte[] GetHash(string inputString) {
            var bytes = Encoding.UTF8.GetBytes(inputString);

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(bytes);
        }

        /// <summary>
        /// Applies SHA256 algorithm to hash <paramref name="password"/>, and returns the password as hashed <c>string</c>.
        /// </summary>
        /// <param name="password">A raw password to hash.</param>
        /// <returns>The hashed password.</returns>
        public static string GetHashedPassword(string password) {
            var stringBuilder = new StringBuilder();
            foreach (var hashedByte in GetHash(password)) {
                stringBuilder.Append(hashedByte.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
