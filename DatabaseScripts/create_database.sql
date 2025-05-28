-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'student_registration')
BEGIN
    CREATE DATABASE student_registration;
END;
GO

USE student_registration;
GO

-- Tabla para Estudiantes
CREATE TABLE students (
    student_id INT IDENTITY(1,1) PRIMARY KEY, 
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    registration_date DATETIME DEFAULT GETDATE(),
	last_updated DATETIME DEFAULT GETDATE()
);
GO

-- Tabla para Materias
CREATE TABLE subjects (
    subject_id INT IDENTITY(1,1) PRIMARY KEY,
    subject_name VARCHAR(100) NOT NULL UNIQUE,
    credits INT NOT NULL DEFAULT 3
);
GO

-- Tabla para Profesores
CREATE TABLE professors (
    professor_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL
);
GO

-- Tabla para relacionar Profesores y Materias (relación muchos a muchos)
CREATE TABLE professor_subjects (
    professor_id INT NOT NULL,
    subject_id INT NOT NULL,
    PRIMARY KEY (professor_id, subject_id),
    FOREIGN KEY (professor_id) REFERENCES professors(professor_id),
    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id)
);
GO

-- Tabla para la inscripción de Estudiantes en Materias
CREATE TABLE student_subjects (
    student_subject_id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    subject_id INT NOT NULL,
    professor_id INT NOT NULL, 
    enrollment_date DATETIME DEFAULT GETDATE(),
    CONSTRAINT UQ_StudentSubject UNIQUE (student_id, subject_id), 
    FOREIGN KEY (student_id) REFERENCES students(student_id),
    FOREIGN KEY (subject_id) REFERENCES subjects(subject_id),
    FOREIGN KEY (professor_id) REFERENCES professors(professor_id)
);
GO