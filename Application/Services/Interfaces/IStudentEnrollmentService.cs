using StudentRegistration.Application.DTOs;

namespace StudentRegistration.Application.Services.Interfaces
{
    public interface IStudentEnrollmentService
    {
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
        Task<IEnumerable<ProfessorDto>> GetAllProfessorsAsync();
        Task<bool> EnrollStudentInSubjectsAsync(EnrollStudentDto enrollStudentDto);
        Task<IEnumerable<StudentEnrollmentDto>> GetStudentEnrollmentsAsync(int studentId);
        Task<IEnumerable<SharedClassStudentDto>> GetStudentsInSharedClassAsync(int subjectId, int currentStudentId);
        Task<IEnumerable<StudentEnrollmentDto>> GetAllStudentEnrollmentsAsync();
    }
}
