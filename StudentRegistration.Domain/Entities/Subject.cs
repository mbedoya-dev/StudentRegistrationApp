namespace StudentRegistration.Domain.Entities
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }

        // Propiedades de navegación
        public ICollection<ProfessorSubject> ProfessorSubjects { get; set; } = new List<ProfessorSubject>();
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
