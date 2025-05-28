namespace StudentRegistration.Domain.Entities
{
    public class Professor
    {
        public int ProfessorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Propiedad de navegación
        public ICollection<ProfessorSubject> ProfessorSubjects { get; set; } = new List<ProfessorSubject>();
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
