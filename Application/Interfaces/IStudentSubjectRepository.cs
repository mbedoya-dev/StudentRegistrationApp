using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Interfaces
{
    public interface IStudentSubjectRepository
    {
        Task AddRangeAsync(IEnumerable<StudentSubject> studentSubjects);
        Task<IEnumerable<StudentSubject>> GetStudentEnrollmentsWithDetailsAsync(int studentId);
        Task<IEnumerable<StudentSubject>> GetAllEnrollmentsWithDetailsAsync();
        Task<IEnumerable<StudentSubject>> GetStudentsInSubjectWithDetailsAsync(int subjectId, int? excludeStudentId = null);
        Task<bool> HasStudentProfessorInEnrollmentsAsync(int studentId, int professorId);
        Task<bool> IsStudentEnrolledInSubjectAsync(int studentId, int subjectId);
        Task<IEnumerable<StudentSubject>> GetStudentEnrollmentsCountAsync(int studentId);
    }
}
