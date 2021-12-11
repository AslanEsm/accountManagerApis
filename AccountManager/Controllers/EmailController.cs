using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using WebFramework.Filters;

namespace AccountManager.Controllers
{
    [ApiResultFilter]
    public class EmailController : SiteBaseController
    {
        #region Constructor

        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        #endregion Constructor

        //api/email/SendEmail
        [HttpPost("[action]")]
        public async Task<IActionResult> SendEmail(string[] emails, string subject, string message)
        {
            await _emailService.SendEmailsAsync(emails, subject, message);
            return Ok();
        }
    }
}