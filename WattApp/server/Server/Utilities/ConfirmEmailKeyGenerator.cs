using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Server.Utilities
{
    public class ConfirmEmailKeyGenerator
    {
        public static string GenerateConfirmEmailKey(int length = 20)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?`~";

            using (var rng = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                rng.GetBytes(data);

                var result = new StringBuilder(length);

                for (int i = 0; i < length; i++)
                {
                    var index = data[i] % chars.Length;
                    result.Append(chars[index]);
                }

                return result.ToString();
            }
        }


    }
}
