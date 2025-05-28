namespace StudentRegistration.Domain.Entities
{
    public class ProfessorSubject
    {
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; } = null!; // Propiedad de navegación

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!; // Propiedad de navegación
    }
}
