namespace Server.Services
{
    /// <summary>
    /// Email service
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="destination">destination email</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        void SendEmail(string destination, string subject, string body, bool isBodyHtml = false);

    }
}
