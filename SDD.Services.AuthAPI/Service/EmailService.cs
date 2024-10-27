using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SDD.Services.AuthAPI.Service.IService;
using SDD.Services.AuthAPI.Util;

namespace SDD.Services.AuthAPI.Service
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var fromEmail = Constants.emailFrom;
            var fromPassword = Constants.emailPassword;

            try
            {
                // Create a new MimeMessage
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;                         

                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = message
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(Constants.emailHost, Constants.emailPort, SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(fromEmail, fromPassword);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
