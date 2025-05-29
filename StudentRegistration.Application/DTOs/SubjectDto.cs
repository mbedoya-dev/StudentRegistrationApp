namespace StudentRegistration.Application.DTOs
{
    public class SubjectDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }
    }

    public class SubjectDetailDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public List<ProfessorDto> AvailableProfessors { get; set; } = new List<ProfessorDto>();
    }
}
