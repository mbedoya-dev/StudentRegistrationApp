using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Services.Interfaces;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los estudiantes registrados.
        /// </summary>
        /// <returns>Una lista de objetos StudentDto.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los estudiantes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al obtener estudiantes.");
            }
        }

        /// <summary>
        /// Obtiene un estudiante por su ID.
        /// </summary>
        /// <param name="id">El ID del estudiante.</param>
        /// <returns>Un objeto StudentDto si se encuentra, o NotFound si no.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    _logger.LogWarning("Estudiante con ID {StudentId} no encontrado.", id);
                    return NotFound($"Estudiante con ID {id} no encontrado.");
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estudiante por ID: {StudentId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor al obtener estudiante con ID {id}.");
            }
        }

        /// <summary>
        /// Registra un nuevo estudiante.
        /// </summary>
        /// <param name="createStudentDto">Datos del nuevo estudiante.</param>
        /// <returns>El StudentDto del estudiante creado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // Para correos duplicados
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> RegisterStudent([FromBody] CreateStudentDto createStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var student = await _studentService.RegisterStudentAsync(createStudentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflicto al registrar estudiante: {Message}", ex.Message);
                return Conflict(ex.Message); // 409 Conflict para correos duplicados
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error de aplicación al registrar estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al registrar estudiante.");
            }
        }

        /// <summary>
        /// Actualiza los datos de un estudiante existente.
        /// </summary>
        /// <param name="id">El ID del estudiante a actualizar.</param>
        /// <param name="updateStudentDto">Los datos actualizados del estudiante.</param>
        /// <returns>El StudentDto del estudiante actualizado.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // Para correos duplicados
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> UpdateStudent(int id, [FromBody] UpdateStudentDto updateStudentDto)
        {
            if (id != updateStudentDto.StudentId)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del estudiante en el cuerpo de la solicitud.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedStudent = await _studentService.UpdateStudentAsync(updateStudentDto);
                if (updatedStudent == null)
                {
                    _logger.LogWarning("Estudiante con ID {StudentId} no encontrado para actualizar.", id);
                    return NotFound($"Estudiante con ID {id} no encontrado.");
                }
                return Ok(updatedStudent);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflicto al actualizar estudiante: {Message}", ex.Message);
                return Conflict(ex.Message); // 409 Conflict para correos duplicados
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error de aplicación al actualizar estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al actualizar estudiante.");
            }
        }

        /// <summary>
        /// Elimina un estudiante por su ID.
        /// </summary>
        /// <param name="id">El ID del estudiante a eliminar.</param>
        /// <returns>NoContent si se elimina exitosamente, o NotFound si no se encuentra.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Estudiante con ID {StudentId} no encontrado para eliminar.", id);
                    return NotFound($"Estudiante con ID {id} no encontrado.");
                }
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error de aplicación al eliminar estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar estudiante.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al eliminar estudiante.");
            }
        }
    }
}
