namespace MailKitDemo.Configurations
{
    public class EmailConfiguration
    {
        public int Port { get; set; }
        public string Host { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
    }
}
