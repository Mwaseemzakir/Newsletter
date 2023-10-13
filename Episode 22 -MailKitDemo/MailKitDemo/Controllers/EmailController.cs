using MailKitDemo.DTOs;
using MailKitDemo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MailKitDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost(Name = "SendEmail")]
        public void SendEmail(SendEmailDto emailDto)
        {
            _emailService.SendEmail(emailDto);
        }
    }
}
