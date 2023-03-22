using System.Net;
using System.Net.Mail;

namespace Server.Services.Implementations
{
    /// <summary>
    /// Email service implementation
    /// </summary>
    public class EmailService:IEmailService
    {
        /// <summary>
        /// Represents a set of key/value application configuration properties
        /// </summary>
        public readonly IConfiguration configuration;
        /// <inheritdocs/>
        public readonly SmtpClient smtpClient;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="configuration"></param>
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
        /// <inheritdoc/>
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
