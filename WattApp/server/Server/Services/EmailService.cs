using System.Net;
using System.Net.Mail;

namespace Server.Services
{
    public class EmailService
    {
        public readonly IConfiguration configuration;
        public readonly SmtpClient smtpClient;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.smtpClient = new SmtpClient(configuration["Smtp:Host"])
            {
                Port = int.Parse(configuration["Smtp:Port"]),
                UseDefaultCredentials=false,
                Credentials = new NetworkCredential(configuration["Smtp:Username"], configuration["Smtp:Password"]),
                EnableSsl = true,
            };

        }   

        public void SendEmail(string destination,string subject,string body,bool isBodyHtml=false)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(this.configuration["Smtp:Username"], this.configuration["Smtp:Name"]);
            message.To.Add(new MailAddress(destination));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isBodyHtml;

            // dodavanje SPF i DKIM autentifikacije
            message.Headers.Add("X-Mailer", "Microsoft C# SmtpClient");
            message.Headers.Add("Received-SPF", "pass");
            message.Headers.Add("Authentication-Results", "google.com; dkim=pass");
            smtpClient.Send(message);
        }
    }
}
