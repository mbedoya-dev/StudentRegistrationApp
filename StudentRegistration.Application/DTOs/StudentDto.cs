 namespace StudentRegistration.Application.DTOs
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
