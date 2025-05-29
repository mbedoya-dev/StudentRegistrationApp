using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Services.Interfaces;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentEnrollmentsController : ControllerBase
    {
        private readonly IStudentEnrollmentService _studentEnrollmentService;
        private readonly ILogger<StudentEnrollmentsController> _logger;

        public StudentEnrollmentsController(IStudentEnrollmentService studentEnrollmentService, ILogger<StudentEnrollmentsController> logger)
        {
            _studentEnrollmentService = studentEnrollmentService;
            _logger = logger;
        }

        /// <summary>
        /// Inscribe a un estudiante en una o más materias.
        /// </summary>
        /// <param name="enrollStudentDto">DTO con el ID del estudiante y la lista de inscripciones (materia y profesor).</param>
        /// <returns>Estado 200 OK si la inscripción es exitosa.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // Para reglas de negocio violadas
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollStudentDto enrollStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _studentEnrollmentService.EnrollStudentInSubjectsAsync(enrollStudentDto);
                if (result)
                {
                    return Ok(new { message = "Inscripción realizada exitosamente." });
                }
                return BadRequest("No se pudo completar la inscripción."); // Esto debería ser manejado por excepciones más específicas
            }
            catch (ArgumentException ex) // Para errores de datos de entrada como estudiante no existente
            {
                _logger.LogWarning(ex, "Error de argumento al inscribir estudiante: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex) // Para reglas de negocio violadas (ej. 3 materias, profesor repetido)
            {
                _logger.LogWarning(ex, "Conflicto de negocio al inscribir estudiante: {Message}", ex.Message);
                return Conflict(ex.Message); // 409 Conflict
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error de aplicación al inscribir estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al inscribir estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al inscribir estudiante.");
            }
        }

        /// <summary>
        /// Obtiene todas las inscripciones de un estudiante específico.
        /// </summary>
        /// <param name="studentId">El ID del estudiante.</param>
        /// <returns>Una lista de StudentEnrollmentDto.</returns>
        [HttpGet("student/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentEnrollmentDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentEnrollmentDto>>> GetStudentEnrollments(int studentId)
        {
            try
            {
                var enrollments = await _studentEnrollmentService.GetStudentEnrollmentsAsync(studentId);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener inscripciones para el estudiante {StudentId}.", studentId);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor al obtener inscripciones para el estudiante {studentId}.");
            }
        }

        /// <summary>
        /// Obtiene una lista de todos los registros de inscripción de estudiantes (para ver registros de otros estudiantes).
        /// </summary>
        /// <returns>Una lista de StudentEnrollmentDto.</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentEnrollmentDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentEnrollmentDto>>> GetAllStudentEnrollments()
        {
            try
            {
                var allEnrollments = await _studentEnrollmentService.GetAllStudentEnrollmentsAsync();
                return Ok(allEnrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de inscripciones de estudiantes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al obtener todos los registros de inscripciones.");
            }
        }

        /// <summary>
        /// Obtiene los nombres de los alumnos que comparten una clase específica con el estudiante actual.
        /// </summary>
        /// <param name="subjectId">El ID de la materia.</param>
        /// <param name="currentStudentId">El ID del estudiante actual (para excluirlo de la lista).</param>
        /// <returns>Una lista de SharedClassStudentDto.</returns>
        [HttpGet("shared-class/{subjectId}/excluding/{currentStudentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SharedClassStudentDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SharedClassStudentDto>>> GetStudentsInSharedClass(int subjectId, int currentStudentId)
        {
            try
            {
                var sharedStudents = await _studentEnrollmentService.GetStudentsInSharedClassAsync(subjectId, currentStudentId);
                return Ok(sharedStudents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estudiantes que comparten la clase {SubjectId} para el estudiante {CurrentStudentId}.", subjectId, currentStudentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al obtener estudiantes que comparten clase.");
            }
        }
    }
}
