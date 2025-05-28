using Microsoft.Extensions.Logging;
using AutoMapper;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Application.Services.Interfaces;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Application.DTOs;


namespace StudentRegistration.Application.Services.Implementations
{
    public class StudentEnrollmentService : IStudentEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentEnrollmentService> _logger;

        public StudentEnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentEnrollmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            try
            {
                var subjects = await _unitOfWork.Subjects.GetAllAsync();
                return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las materias.");
                return new List<SubjectDto>();
            }
        }

        public async Task<IEnumerable<ProfessorDto>> GetAllProfessorsAsync()
        {
            try
            {
                var professors = await _unitOfWork.Professors.GetAllAsync();
                return _mapper.Map<IEnumerable<ProfessorDto>>(professors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los profesores.");
                return new List<ProfessorDto>();
            }
        }

        public async Task<bool> EnrollStudentInSubjectsAsync(EnrollStudentDto enrollStudentDto)
        {
            try
            {
                // 1. Validar que el estudiante exista
                var student = await _unitOfWork.Students.GetByIdAsync(enrollStudentDto.StudentId);
                if (student == null)
                {
                    _logger.LogWarning("Intento de inscripción para estudiante no existente: {StudentId}", enrollStudentDto.StudentId);
                    throw new ArgumentException("El estudiante especificado no existe.");
                }

                // Obtener el conteo actual de inscripciones del estudiante
                var existingEnrollmentsCount = (await _unitOfWork.StudentSubjects.GetStudentEnrollmentsCountAsync(enrollStudentDto.StudentId)).Count();

                // 2. Validar el límite de 3 materias (considerando las nuevas y las existentes)
                if (existingEnrollmentsCount + enrollStudentDto.Enrollments.Count > 3)
                {
                    _logger.LogWarning("Intento de inscripción excede el límite de 3 materias para estudiante: {StudentId}", enrollStudentDto.StudentId);
                    throw new InvalidOperationException($"Un estudiante solo puede seleccionar un máximo de 3 materias en total. Ya tiene {existingEnrollmentsCount} inscritas.");
                }

                // Obtener todos los profesores ya asignados a este estudiante (en inscripciones existentes)
                var existingProfessorIdsForStudent = (await _unitOfWork.StudentSubjects.GetStudentEnrollmentsWithDetailsAsync(enrollStudentDto.StudentId))
                                                                .Select(e => e.ProfessorId)
                                                                .ToHashSet();

                var newEnrollments = new List<StudentSubject>();
                var newProfessorIdsInThisBatch = new HashSet<int>();
                var newSubjectIdsInThisBatch = new HashSet<int>();

                foreach (var enrollmentDetail in enrollStudentDto.Enrollments)
                {
                    // Validar que la materia y el profesor existan
                    var subject = await _unitOfWork.Subjects.GetByIdAsync(enrollmentDetail.SubjectId);
                    var professor = await _unitOfWork.Professors.GetByIdAsync(enrollmentDetail.ProfessorId);

                    if (subject == null || professor == null)
                    {
                        _logger.LogWarning("Intento de inscripción con materia o profesor inválido. MateriaId: {SubjectId}, ProfesorId: {ProfessorId}", enrollmentDetail.SubjectId, enrollmentDetail.ProfessorId);
                        throw new ArgumentException("Materia o profesor inválido en la solicitud de inscripción.");
                    }

                    // Validar que la materia no esté ya inscrita por el estudiante
                    if (await _unitOfWork.StudentSubjects.IsStudentEnrolledInSubjectAsync(enrollStudentDto.StudentId, enrollmentDetail.SubjectId))
                    {
                        _logger.LogWarning("Intento de reinscripción en materia ya cursada por estudiante: {StudentId}, MateriaId: {SubjectId}", enrollStudentDto.StudentId, enrollmentDetail.SubjectId);
                        throw new InvalidOperationException($"El estudiante ya está inscrito en la materia con ID {enrollmentDetail.SubjectId}.");
                    }

                    // Validar que la materia no esté duplicada en la misma solicitud
                    if (newSubjectIdsInThisBatch.Contains(enrollmentDetail.SubjectId))
                    {
                        throw new InvalidOperationException($"La materia con ID {enrollmentDetail.SubjectId} está duplicada en la solicitud de inscripción actual.");
                    }
                    newSubjectIdsInThisBatch.Add(enrollmentDetail.SubjectId);


                    // Validar que el profesor no esté ya asignado a otra materia del estudiante (existente o en este mismo lote)
                    if (existingProfessorIdsForStudent.Contains(enrollmentDetail.ProfessorId) || newProfessorIdsInThisBatch.Contains(enrollmentDetail.ProfessorId))
                    {
                        _logger.LogWarning("Intento de inscripción con profesor ya asignado a otra materia del estudiante: {StudentId}, ProfesorId: {ProfessorId}", enrollStudentDto.StudentId, enrollmentDetail.ProfessorId);
                        throw new InvalidOperationException($"El estudiante no puede tener clases con el mismo profesor (ID: {enrollmentDetail.ProfessorId}) en diferentes materias.");
                    }

                    var professorTeachesSubject = await _unitOfWork.Professors.GetByIdAsync(enrollmentDetail.ProfessorId);
                    
                    if (professorTeachesSubject != null && !professorTeachesSubject.ProfessorSubjects.Any(ps => ps.SubjectId == enrollmentDetail.SubjectId))
                    {
                        _logger.LogWarning("El profesor {ProfessorId} no dicta la materia {SubjectId}.", enrollmentDetail.ProfessorId, enrollmentDetail.SubjectId);
                        throw new InvalidOperationException($"El profesor seleccionado (ID: {enrollmentDetail.ProfessorId}) no dicta la materia (ID: {enrollmentDetail.SubjectId}).");
                    }

                    newEnrollments.Add(new StudentSubject
                    {
                        StudentId = enrollStudentDto.StudentId,
                        SubjectId = enrollmentDetail.SubjectId,
                        ProfessorId = enrollmentDetail.ProfessorId,
                        EnrollmentDate = DateTime.Now
                    });

                    newProfessorIdsInThisBatch.Add(enrollmentDetail.ProfessorId); // Añadir a los profesores de esta nueva tanda de inscripciones
                    existingProfessorIdsForStudent.Add(enrollmentDetail.ProfessorId); // Marcar el profesor como usado para futuras validaciones en el mismo lote
                }

                await _unitOfWork.StudentSubjects.AddRangeAsync(newEnrollments);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Estudiante {StudentId} inscrito exitosamente en {Count} materias.", enrollStudentDto.StudentId, enrollStudentDto.Enrollments.Count);
                return true;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de argumento al inscribir estudiante.");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error de lógica de negocio al inscribir estudiante.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al inscribir estudiante en materias.");
                throw new ApplicationException("Ocurrió un error inesperado al inscribir al estudiante.", ex);
            }
        }


        public async Task<IEnumerable<StudentEnrollmentDto>> GetStudentEnrollmentsAsync(int studentId)
        {
            try
            {
                var enrollments = await _unitOfWork.StudentSubjects.GetStudentEnrollmentsWithDetailsAsync(studentId);
                return _mapper.Map<IEnumerable<StudentEnrollmentDto>>(enrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inscripciones para el estudiante: {StudentId}", studentId);
                return new List<StudentEnrollmentDto>();
            }
        }

        public async Task<IEnumerable<SharedClassStudentDto>> GetStudentsInSharedClassAsync(int subjectId, int currentStudentId)
        {
            try
            {
                var studentsInClass = await _unitOfWork.StudentSubjects.GetStudentsInSubjectWithDetailsAsync(subjectId, currentStudentId);
                return _mapper.Map<IEnumerable<SharedClassStudentDto>>(studentsInClass.Select(ss => ss.Student).Distinct());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estudiantes que comparten la clase {SubjectId} para el estudiante {CurrentStudentId}.", subjectId, currentStudentId);
                return new List<SharedClassStudentDto>();
            }
        }

        public async Task<IEnumerable<StudentEnrollmentDto>> GetAllStudentEnrollmentsAsync()
        {
            try
            {
                var allEnrollments = await _unitOfWork.StudentSubjects.GetAllEnrollmentsWithDetailsAsync();
                return _mapper.Map<IEnumerable<StudentEnrollmentDto>>(allEnrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de inscripciones de estudiantes.");
                return new List<StudentEnrollmentDto>();
            }
        }
    }
}
