namespace MailKitDemo.DTOs
{
    public class SendEmailDto
    {
        public string To { get; set; } = string.Empty;
        public string PlainText { get; set; } = string.Empty;
        public string Html { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}
