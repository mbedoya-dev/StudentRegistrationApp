namespace StudentRegistration.Application.DTOs
{
    public class ProfessorDto
    {
        public int ProfessorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
