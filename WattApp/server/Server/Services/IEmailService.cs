namespace Server.Services
{
    public interface IEmailService
    {
        void SendEmail(string destination, string subject, string body, bool isBodyHtml = false);
    }
}
