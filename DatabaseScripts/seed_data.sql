-- Insertar las 10 materias iniciales
INSERT INTO subjects (subject_name) VALUES
('Matemáticas I'),
('Física I'),
('Química I'),
('Fundamentos de Ciencias de la Computación'),
('Fundamentos de Desarrollo Web'),
('Gestión de Bases de Datos'),
('Algoritmos y Estructuras de Datos'),
('Programación Orientada a Objetos'),
('Ingeniería de Software'),
('Gestión de Proyectos');
GO

-- Insertar los 5 profesores iniciales
INSERT INTO professors (first_name, last_name, email) VALUES
('Ana', 'García', 'ana.garcia@uni.com'),
('Juan', 'Rodríguez', 'juan.rodriguez@uni.com'),
('María', 'Fernández', 'maria.fernandez@uni.com'),
('Pedro', 'López', 'pedro.lopez@uni.com'),
('Laura', 'Martínez', 'laura.martinez@uni.com');
GO

-- Asignar 2 materias a cada profesor
INSERT INTO professor_subjects (professor_id, subject_id) VALUES
(1, 1), (1, 2), -- Ana García dicta Matemáticas I, Física I
(2, 3), (2, 4), -- Juan Rodríguez dicta Química I, Fundamentos de Ciencias de la Computación
(3, 5), (3, 6), -- María Fernández dicta Fundamentos de Desarrollo Web, Gestión de Bases de Datos
(4, 7), (4, 8), -- Pedro López dicta Algoritmos y Estructuras de Datos, Programación Orientada a Objetos
(5, 9), (5, 10); -- Laura Martínez dicta Ingeniería de Software, Gestión de Proyectos
GO

