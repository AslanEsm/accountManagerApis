using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Services.Interfaces;

namespace Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await _emailSender.SendEmailAsync(email, subject, message);
        }

        public async Task SendEmailsAsync(string[] emails, string subject, string message)
        {
            if (emails != null)
            {
                foreach (var email in emails)
                {
                    await _emailSender.SendEmailAsync(email, subject, message);
                }
            }
        }

    }
}
