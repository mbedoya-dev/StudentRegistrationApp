
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Application.Services.Interfaces;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.GetByIdAsync(id);
                return _mapper.Map<StudentDto>(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estudiante por ID: {StudentId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            try
            {
                var students = await _unitOfWork.Students.GetAllAsync();
                return _mapper.Map<IEnumerable<StudentDto>>(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los estudiantes.");
                return new List<StudentDto>();
            }
        }

        public async Task<StudentDto> RegisterStudentAsync(CreateStudentDto createStudentDto)
        {
            try
            {
                // Validar si el correo ya existe
                var existingStudent = await _unitOfWork.Students.GetByEmailAsync(createStudentDto.Email);
                if (existingStudent != null)
                {
                    _logger.LogWarning("Intento de registrar estudiante con correo ya existente: {Email}", createStudentDto.Email);
                    throw new InvalidOperationException("Ya existe un estudiante con este correo electrónico.");
                }

                var student = _mapper.Map<Student>(createStudentDto);
                student.RegistrationDate = DateTime.Now;
                student.LastUpdated = DateTime.Now;

                await _unitOfWork.Students.AddAsync(student);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Estudiante registrado exitosamente: {StudentId}", student.StudentId);
                return _mapper.Map<StudentDto>(student);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error de lógica de negocio al registrar estudiante.");
                throw; // Re-lanzar para que el controlador lo maneje
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar estudiante.");
                throw new ApplicationException("Ocurrió un error inesperado al registrar el estudiante.", ex);
            }
        }

        public async Task<StudentDto?> UpdateStudentAsync(UpdateStudentDto updateStudentDto)
        {
            try
            {
                var studentToUpdate = await _unitOfWork.Students.GetByIdAsync(updateStudentDto.StudentId);
                if (studentToUpdate == null)
                {
                    _logger.LogWarning("Intento de actualizar estudiante no existente: {StudentId}", updateStudentDto.StudentId);
                    return null; // O lanzar una excepción específica
                }

                // Validar si el nuevo correo ya existe y pertenece a otro estudiante
                var existingStudentWithEmail = await _unitOfWork.Students.GetByEmailAsync(updateStudentDto.Email);
                if (existingStudentWithEmail != null && existingStudentWithEmail.StudentId != updateStudentDto.StudentId)
                {
                    _logger.LogWarning("Intento de actualizar estudiante con correo ya existente por otro estudiante: {Email}", updateStudentDto.Email);
                    throw new InvalidOperationException("El correo electrónico ya está en uso por otro estudiante.");
                }

                _mapper.Map(updateStudentDto, studentToUpdate);
                studentToUpdate.LastUpdated = DateTime.Now;

                _unitOfWork.Students.Update(studentToUpdate);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Estudiante actualizado exitosamente: {StudentId}", studentToUpdate.StudentId);
                return _mapper.Map<StudentDto>(studentToUpdate);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error de lógica de negocio al actualizar estudiante.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar estudiante.");
                throw new ApplicationException("Ocurrió un error inesperado al actualizar el estudiante.", ex);
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                var studentToDelete = await _unitOfWork.Students.GetByIdAsync(id);
                if (studentToDelete == null)
                {
                    _logger.LogWarning("Intento de eliminar estudiante no existente: {StudentId}", id);
                    return false;
                }

                _unitOfWork.Students.Delete(studentToDelete);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Estudiante eliminado exitosamente: {StudentId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar estudiante: {StudentId}", id);
                throw new ApplicationException("Ocurrió un error inesperado al eliminar el estudiante.", ex);
            }
        }
    }
}
