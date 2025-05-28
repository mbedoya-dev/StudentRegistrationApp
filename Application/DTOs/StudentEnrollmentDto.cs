namespace StudentRegistration.Application.DTOs
{
    public class StudentEnrollmentDto
    {
        public int StudentSubjectId { get; set; }
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; } = string.Empty;
        public string StudentLastName { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int ProfessorId { get; set; }
        public string ProfessorFirstName { get; set; } = string.Empty;
        public string ProfessorLastName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
    }
}
