-- Insertar las 5 materias
INSERT INTO subjects (subject_name) VALUES
('Matemáticas I'),
('Física I'),
('Química I'),
('Fundamentos de Desarrollo Web'),
('Gestión de Bases de Datos');
GO

-- Insertar los 5 profesores
INSERT INTO professors (first_name, last_name, email) VALUES
('Ana', 'García', 'ana.garcia@example.com'),           -- ID 1
('Juan', 'Rodríguez', 'juan.rodriguez@example.com'),   -- ID 2
('María', 'Fernández', 'maria.fernandez@example.com'), -- ID 3
('Pedro', 'López', 'pedro.lopez@example.com'),         -- ID 4
('Laura', 'Martínez', 'laura.martinez@example.com');   -- ID 5
GO

-- Asignaciones
INSERT INTO professor_subjects (professor_id, subject_id) VALUES
(1, 1), -- Ana - Matemáticas I
(2, 1), -- Juan - Matemáticas I

(1, 2), -- Ana - Física I
(3, 2), -- María - Física I

(2, 3), -- Juan - Química I
(4, 3), -- Pedro - Química I

(3, 4), -- María - Desarrollo Web
(5, 4), -- Laura - Desarrollo Web

(4, 5), -- Pedro - Bases de Datos
(5, 5); -- Laura - Bases de Datos
GO