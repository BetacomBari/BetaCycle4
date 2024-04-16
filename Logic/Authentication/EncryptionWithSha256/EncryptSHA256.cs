
using System.Security.Cryptography;
using System.Text;

namespace BetaCycle4.Logic.Authentication.EncryptionWithSha256
{
    public class EncryptionSHA256
    {
        public static string sha256Encrypt(string s)
        {
            string result = string.Empty;
            var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            if (s != null)
            {
                try
                {
                    byte[] hash = sha256.ComputeHash(bytes);
                    result = BitConverter.ToString(hash, 0, hash.Length);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }
    }
}
