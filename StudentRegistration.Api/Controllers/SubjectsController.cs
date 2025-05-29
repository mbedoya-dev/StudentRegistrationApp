using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Services.Interfaces;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly IStudentEnrollmentService _studentEnrollmentService;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(IStudentEnrollmentService studentEnrollmentService, ILogger<SubjectsController> logger)
        {
            _studentEnrollmentService = studentEnrollmentService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las materias disponibles.
        /// </summary>
        /// <returns>Una lista de objetos SubjectDto.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubjectDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAllSubjects()
        {
            try
            {
                var subjects = await _studentEnrollmentService.GetAllSubjectsAsync();
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las materias.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al obtener materias.");
            }
        }

        /// <summary>
        /// Obtiene todas las materias disponibles con un profesor asignado
        /// </summary>
        /// <returns>Una lista de objetos SubjectDetailDto.</returns>
        [HttpGet("with-available-professors")] // Nueva ruta
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubjectDetailDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SubjectDetailDto>>> GetAllSubjectsWithAssignedProfessor()
        {
            try
            {
                var subjectsWithDetails = await _studentEnrollmentService.GetAllSubjectsWithAvailableProfessorsAsync();
                return Ok(subjectsWithDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las materias con profesor asignado.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al obtener materias con profesor asignado.");
            }
        }

        /// <summary>
        /// Obtiene todos los profesores disponibles.
        /// </summary>
        /// <returns>Una lista de objetos ProfessorDto.</returns>
        [HttpGet("professors")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProfessorDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProfessorDto>>> GetAllProfessors()
        {
            try
            {
                var professors = await _studentEnrollmentService.GetAllProfessorsAsync();
                return Ok(professors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los profesores.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al obtener profesores.");
            }
        }
    }
}
