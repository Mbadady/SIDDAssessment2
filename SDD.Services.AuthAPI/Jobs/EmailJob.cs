using SDD.Services.AuthAPI.Service.IService;

namespace SDD.Services.AuthAPI.Jobs
{
    public class EmailJob
    {
        private readonly IEmailService _emailService;

        public EmailJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendEmailJob(string to, string subject, string body)
        {
            await _emailService.SendEmailAsync(to, subject, body);
        }
    }
}
