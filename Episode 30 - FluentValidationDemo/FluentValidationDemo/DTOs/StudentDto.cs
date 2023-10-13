namespace FluentValidationDemo.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
