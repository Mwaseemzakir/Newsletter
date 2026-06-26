using MailKitSample.DTOs;

namespace MailKitSample.Services.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(SendEmailDto emailDto);
    }
}
