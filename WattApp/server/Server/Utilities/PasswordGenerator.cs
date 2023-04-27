using System.Text;

namespace Server.Utilities
{
    /// <summary>
    /// Password generator
    /// </summary>
    public class PasswordGenerator
    {
        /// <summary>
        /// Generate random password
        /// </summary>
        /// <param name="length">Length of password</param>
        /// <returns>Password</returns>
        public static string GenerateRandomPassword(int length = 10)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
