# Prueba Técnica Inter Rapídisimo - Sistema de Registro de Estudiantes

Este proyecto es una aplicación Full-Stack desarrollada como parte de la prueba técnica para Inter Rapídisimo. La aplicación permite el registro de estudiantes y su inscripción en materias, siguiendo una serie de reglas de negocio específicas.

**Desarrollado por:** Mauricio Bedoya Galeano
**Contacto:** mbedgal@gmail.com
**Perfil de GitHub:** [https://github.com/mbedoya-dev](https://github.com/mbedoya-dev)

## Descripción General

La solución consta de tres componentes principales:

1.  **Base de Datos (SQL Server):** Almacena la información de estudiantes, materias, profesores y las relaciones entre ellos.
2.  **API RESTful (.NET Core):** Proporciona los endpoints para gestionar las operaciones de la aplicación.
3.  **Aplicación Web (Angular):** Interfaz de usuario para interactuar con el sistema.

## Características Implementadas

### Backend (.NET Core API)

* **Endpoints para Estudiantes:**
    * `GET /api/Students`: Obtiene todos los estudiantes.
    * `GET /api/Students/{id}`: Obtiene un estudiante por su ID.
    * `POST /api/Students`: Registra un nuevo estudiante.
    * `PUT /api/Students/{id}`: Actualiza un estudiante existente.
    * `DELETE /api/Students/{id}`: Elimina un estudiante.
* **Endpoints para Inscripciones y Materias (a través de `StudentEnrollmentsController` y `SubjectsController`):**
    * `POST /api/StudentEnrollments`: Inscribe a un estudiante en una o más materias.
    * `GET /api/StudentEnrollments/student/{studentId}`: Obtiene todas las inscripciones de un estudiante.
    * `GET /api/StudentEnrollments/all`: Obtiene todos los registros de inscripción.
    * `GET /api/StudentEnrollments/shared-class/{subjectId}/excluding/{currentStudentId}`: Obtiene los estudiantes que comparten una clase (excluyendo al estudiante actual).
    * `GET /api/Subjects`: Obtiene todas las materias disponibles.
    * `GET /api/Subjects/with-available-professors`: Obtiene materias con los profesores que pueden dictarlas.
    * `GET /api/Subjects/professors`: Obtiene todos los profesores.
* **Lógica de Negocio para Inscripciones:**
    * Un estudiante puede inscribir un máximo de 3 materias.
    * Un estudiante no puede inscribir la misma materia más de una vez.
    * Un estudiante no puede tener clases con el mismo profesor en diferentes materias.
    * Un profesor debe estar asignado a una materia para poder dictarla.
* **Manejo de Datos:**
    * Uso de Entity Framework Core para la interacción con la base de datos.
    * Patrón Repositorio y Unidad de Trabajo (Unit of Work) para la abstracción del acceso a datos.
    * Mapeo de objetos entre entidades y DTOs utilizando AutoMapper.
* **Documentación API:**
    * Swagger UI habilitada para la visualización y prueba de los endpoints de la API.
    * Comentarios XML en los controladores para enriquecer la documentación de Swagger.
* **Configuración:**
    * CORS configurado para permitir solicitudes desde la aplicación Angular (localhost:4200).
    * Logging implementado para el seguimiento de eventos y errores.

### Frontend (Angular)

* **Componentes Principales:**
    * **Registro de Estudiantes (`StudentRegistrationComponent`):** Formulario para registrar nuevos estudiantes.
        * Al registrar exitosamente, el estudiante se guarda en una sesión (`StudentSessionService`) y se redirige al componente de inscripción de materias.
    * **Inscripción de Materias (`SubjectEnrollmentComponent`):** Permite a un estudiante (obtenido de la sesión) inscribir materias.
        * Muestra las materias disponibles y los profesores asignados a cada una.
        * Valida las reglas de negocio (máximo 3 materias, no repetir profesor, etc.) antes de enviar la solicitud.
        * Muestra las materias ya inscritas por el estudiante.
    * **Compañeros de Clase (`SharedClassComponent`):** Muestra los compañeros que comparten clases con el estudiante en sesión.
* **Servicios Angular:**
    * `StudentService`: Para operaciones CRUD de estudiantes.
    * `SubjectService`: Para obtener información de materias y profesores.
    * `EnrollmentService`: Para gestionar las inscripciones de los estudiantes.
    * `StudentSessionService`: Maneja la "sesión" del estudiante actualmente registrado o seleccionado en la UI.
* **Interfaz de Usuario:**
    * Basada en el template "Modernize Angular Admin Template".
    * Uso de Angular Material para componentes de UI.
    * Notificaciones Toastr para feedback al usuario.
* **Navegación y Rutas:**
    * Configuración de rutas para los diferentes componentes.
    * Componente de error para rutas no encontradas.

### Base de Datos (SQL Server)

* **Esquema:**
    * `students`: Almacena información de los estudiantes (ID, nombre, apellido, email, fechas).
    * `subjects`: Almacena información de las materias (ID, nombre, créditos).
    * `professors`: Almacena información de los profesores (ID, nombre, apellido, email).
    * `professor_subjects`: Tabla de unión para la relación muchos-a-muchos entre profesores y materias, indicando qué profesor puede dictar qué materia.
    * `student_subjects`: Tabla de inscripción que relaciona estudiantes con materias y el profesor específico que dicta esa materia al estudiante.
* **Scripts:**
    * `create_database.sql`: Script para crear la base de datos y las tablas.
    * `seed_data.sql`: Script para poblar las tablas `subjects`, `professors` y `professor_subjects` con datos iniciales.

## Tecnologías Utilizadas

* **Backend:** .NET Core 8, Entity Framework Core 8, AutoMapper, Swagger (Swashbuckle)
* **Frontend:** Angular 19, Angular Material, TypeScript, RxJS, Ngx-Toastr, Angular Tabler Icons.
* **Base de Datos:** SQL Server

## Configuración y Ejecución

### Prerrequisitos

* .NET SDK 8 o superior.
* SQL Server (Express, Developer, o cualquier otra edición).
* Node.js y npm (para la parte de Angular).
* Angular CLI.

### 1. Configuración de la Base de Datos

1.  Asegúrese de tener una instancia de SQL Server en ejecución.
2.  Actualice la cadena de conexión `DefaultConnection` en el archivo `StudentRegistration.Api/appsettings.json` si es necesario, para que apunte a su instancia de SQL Server y configure las credenciales adecuadas.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=SU_SERVIDOR_SQL;Database=student_registration;User Id=SU_USUARIO;Password=SU_CONTRASENA;Encrypt=False;"
    }
    ```
3.  Ejecute el script `DatabaseScripts/create_database.sql` en su SQL Server para crear la base de datos y las tablas.
4.  Ejecute el script `DatabaseScripts/seed_data.sql` para poblar las tablas con datos iniciales.

### 2. Ejecución del Backend (API)

1.  Navegue a la carpeta `StudentRegistration.Api`.
2.  Compile y ejecute el proyecto:
    ```bash
    dotnet build
    dotnet run
    ```
3.  La API estará disponible (por defecto, según `launchSettings.json`, en `https://localhost:7196` o `http://localhost:5244`).
4.  Puede acceder a la documentación de Swagger en `/swagger`.

### 3. Ejecución del Frontend (Angular)

1.  Navegue a la carpeta `StudentRegistration.Web`.
2.  Instale las dependencias:
    ```bash
    npm install
    ```
3.  Actualice la URL de la API en `StudentRegistration.Web/src/environments/environment.ts` si su API se está ejecutando en un puerto diferente al configurado por defecto (`https://localhost:7196`).
    ```typescript
    export const environment = {
      production: false,
      apiUrl: 'URL_DE_TU_API_AQUI' // Ejemplo: 'https://localhost:7196/api'
    };
    ```
4.  Inicie la aplicación Angular:
    ```bash
    ng serve
    ```
5.  La aplicación web estará disponible en `http://localhost:4200/`.

## Flujo de la Aplicación

1.  **Registro de Estudiante:**
    * El usuario accede a la página principal, que es el formulario de registro de estudiantes.
    * Ingresa nombre, apellido y correo electrónico.
    * Al enviar, se llama al endpoint `POST /api/Students`.
    * Si el registro es exitoso, el estudiante recién creado se guarda en un servicio de sesión (`StudentSessionService`) y el usuario es redirigido a la página de inscripción de materias.
2.  **Inscripción de Materias:**
    * El componente de inscripción obtiene el estudiante actual del `StudentSessionService`. Si no hay estudiante, redirige al registro.
    * Se cargan las materias disponibles (con sus profesores) desde `GET /api/Subjects/with-available-professors` y las inscripciones actuales del estudiante desde `GET /api/StudentEnrollments/student/{studentId}`.
    * El usuario selecciona las materias que desea inscribir (hasta un máximo de 3 en total, contando las ya inscritas).
    * Para cada materia seleccionada, el usuario debe elegir un profesor de la lista de profesores disponibles para esa materia.
    * Se aplican validaciones en el frontend para no exceder el límite de materias y para asegurar que se seleccione un profesor.
    * Al hacer clic en "Inscribir", se construye el payload y se envía a `POST /api/StudentEnrollments`. La API vuelve a validar las reglas de negocio (máximo 3 materias, no repetir profesor entre las nuevas y las existentes, el profesor debe dictar la materia).
    * Se muestra una notificación de éxito o error.
3.  **Ver Compañeros de Clase:**
    * Desde la página de inscripción de materias (si el estudiante ya tiene materias inscritas), hay un botón para "Ver compañeros de clases".
    * Este redirige al componente `SharedClassComponent`.
    * El componente obtiene las inscripciones del estudiante actual y, para cada materia, llama a `GET /api/StudentEnrollments/shared-class/{subjectId}/excluding/{currentStudentId}` para obtener los compañeros.
    * Se muestra una tabla con los nombres de los compañeros y la clase que comparten.

## Estructura del Proyecto

```
/StudentRegistrationApp
|-- /DatabaseScripts
|   |-- create_database.sql
|   |-- seed_data.sql
|-- /StudentRegistration.Api      # Proyecto Backend .NET Core
|   |-- /Controllers
|   |-- /Properties
|   |-- appsettings.json
|   |-- Program.cs
|   |-- StudentRegistration.Api.csproj
|-- /StudentRegistration.Application  # Lógica de negocio, DTOs, Interfaces
|   |-- /DTOs
|   |-- /Interfaces
|   |-- /Mappings
|   |-- /Services
|   |-- StudentRegistration.Application.csproj
|-- /StudentRegistration.Domain     # Entidades del Dominio
|   |-- /Entities
|   |-- StudentRegistration.Domain.csproj
|-- /StudentRegistration.Infrastructure # Repositorios, DbContext
|   |-- /Data
|   |-- /Repositories
|   |-- StudentRegistration.Infrastructure.csproj
|-- /StudentRegistration.Web        # Proyecto Frontend Angular
|   |-- /src
|   |   |-- /app
|   |   |   |-- /components
|   |   |   |-- /layouts
|   |   |   |-- /models
|   |   |   |-- /pages
|   |   |   |-- /services
|   |   |-- /assets
|   |   |-- /environments
|   |-- angular.json
|   |-- package.json
|-- README.md
|-- StudentRegistration.sln
```
