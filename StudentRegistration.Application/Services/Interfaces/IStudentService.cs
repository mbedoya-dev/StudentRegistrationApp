using StudentRegistration.Application.DTOs;

namespace StudentRegistration.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto> RegisterStudentAsync(CreateStudentDto createStudentDto);
        Task<StudentDto?> UpdateStudentAsync(UpdateStudentDto updateStudentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
