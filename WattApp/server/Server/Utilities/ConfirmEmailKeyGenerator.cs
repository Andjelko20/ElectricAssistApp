using System.Text;
using System.Text.RegularExpressions;

namespace Server.Utilities
{
    public class ConfirmEmailKeyGenerator
    {
        public static string GenerateConfirmEmailKey(int lenght = 20)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}\\|;:'\",.<>/?`~";

            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();

            while (lenght > 0)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }

    }
}
