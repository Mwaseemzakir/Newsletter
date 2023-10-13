namespace DapperDemo.DTOs
{
    public record NewsletterDTO
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
    }
}
