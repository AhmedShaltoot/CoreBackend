using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    public static class HashingUtils
    {
        public static string ComputeHash(string input, string secretKey, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using (HMAC hmac = new HMACSHA256(keyBytes))
            {
                byte[] combinedBytes = new byte[inputBytes.Length + keyBytes.Length];
                Buffer.BlockCopy(inputBytes, 0, combinedBytes, 0, inputBytes.Length);
                Buffer.BlockCopy(keyBytes, 0, combinedBytes, inputBytes.Length, keyBytes.Length);

                byte[] hashBytes = algorithm.ComputeHash(combinedBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string ComputeSHA256Hash(string input, string secretKey)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return ComputeHash(input, secretKey, sha256);
            }
        }

        public static string ComputeSHA512Hash(string input, string secretKey)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                return ComputeHash(input, secretKey, sha512);
            }
        }
    }
}
