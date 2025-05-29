
using AutoMapper;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos para Student
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore()) // No actualizar la fecha de registro
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => System.DateTime.Now)); // Actualizar LastUpdated

            // Mapeos para Subject
            CreateMap<Subject, SubjectDto>().ReverseMap();

            // Mapeos para SubjectDetailDto
            CreateMap<Subject, SubjectDetailDto>();

            // Mapeos para Professor
            CreateMap<Professor, ProfessorDto>().ReverseMap();

            // Mapeos para StudentEnrollmentDto (para vistas de inscripciones)
            CreateMap<StudentSubject, StudentEnrollmentDto>()
                .ForMember(dest => dest.StudentFirstName, opt => opt.MapFrom(src => src.Student.FirstName))
                .ForMember(dest => dest.StudentLastName, opt => opt.MapFrom(src => src.Student.LastName))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
                .ForMember(dest => dest.ProfessorFirstName, opt => opt.MapFrom(src => src.Professor.FirstName))
                .ForMember(dest => dest.ProfessorLastName, opt => opt.MapFrom(src => src.Professor.LastName));

            // Mapeos para SharedClassStudentDto
            CreateMap<Student, SharedClassStudentDto>();
        }
    }
}
