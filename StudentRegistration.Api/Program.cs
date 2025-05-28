using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudentRegistration.Application.Interfaces;
using StudentRegistration.Application.Mappings;
using StudentRegistration.Application.Services.Implementations;
using StudentRegistration.Application.Services.Interfaces;
using StudentRegistration.Infrastructure.Data;
using StudentRegistration.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 1. Configuraci�n b�sica de la documentaci�n Swagger
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student Registration API", // T�tulo API
        Version = "v1", // Versi�n API
        Description = "Una API RESTful para gestionar el registro de estudiantes y sus inscripciones en materias.",
        Contact = new OpenApiContact
        {
            Name = "Mauricio Bedoya Galeano",
            Email = "mbedgal@gmail.com",
            Url = new Uri("https://github.com/mbedoya-dev")
        }
    });

    // 2. Configuraci�n para incluir comentarios XML en la documentaci�n Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configuraci�n de la base de datos con Entity Framework Core
builder.Services.AddDbContext<StudentRegistrationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuraci�n de AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Inyecci�n de dependencias para Repositorios e UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IStudentSubjectRepository, StudentSubjectRepository>();

// Inyecci�n de dependencias para Servicios de Aplicaci�n
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentEnrollmentService, StudentEnrollmentService>();

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // URL de tu aplicaci�n Angular
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Registration API V1");
        c.RoutePrefix = "swagger"; // Para servir Swagger UI en la ra�z de la aplicaci�n
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin"); // Usar la pol�tica CORS definida

app.UseAuthorization();

app.MapControllers();

app.Run();