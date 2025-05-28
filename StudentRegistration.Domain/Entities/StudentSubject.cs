namespace StudentRegistration.Domain.Entities
{
    public class StudentSubject
    {
        public int StudentSubjectId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!; // Propiedad de navegación

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!; // Propiedad de navegación

        public int ProfessorId { get; set; }
        public Professor Professor { get; set; } = null!; // Propiedad de navegación

        public DateTime EnrollmentDate { get; set; }
    }
}
