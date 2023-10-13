using MailKitDemo.DTOs;

namespace MailKitDemo.Services.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(SendEmailDto emailDto);
    }
}
