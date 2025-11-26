using System.Security.Cryptography;
using System.Text;

namespace AutoUI.Common.TestItems
{
    public static class CryptoUtils
    {
        public static string MD5Hash(this string input)
        {
            // Use a using statement to ensure the MD5 object is properly disposed
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5Hash.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    // Format each byte as a hexadecimal string (x2 for lowercase)
                    sBuilder.Append(hashBytes[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
