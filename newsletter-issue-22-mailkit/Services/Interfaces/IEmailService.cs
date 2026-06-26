using MailKit.DTOs;

namespace MailKit.Services.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(SendEmailDto emailDto);
    }
}
