-- Insertar las 5 materias
INSERT INTO subjects (subject_name) VALUES
('Matem�ticas I'),
('F�sica I'),
('Qu�mica I'),
('Fundamentos de Desarrollo Web'),
('Gesti�n de Bases de Datos');
GO

-- Insertar los 5 profesores
INSERT INTO professors (first_name, last_name, email) VALUES
('Ana', 'Garc�a', 'ana.garcia@example.com'),           -- ID 1
('Juan', 'Rodr�guez', 'juan.rodriguez@example.com'),   -- ID 2
('Mar�a', 'Fern�ndez', 'maria.fernandez@example.com'), -- ID 3
('Pedro', 'L�pez', 'pedro.lopez@example.com'),         -- ID 4
('Laura', 'Mart�nez', 'laura.martinez@example.com');   -- ID 5
GO

-- Asignaciones
INSERT INTO professor_subjects (professor_id, subject_id) VALUES
(1, 1), -- Ana - Matem�ticas I
(2, 1), -- Juan - Matem�ticas I

(1, 2), -- Ana - F�sica I
(3, 2), -- Mar�a - F�sica I

(2, 3), -- Juan - Qu�mica I
(4, 3), -- Pedro - Qu�mica I

(3, 4), -- Mar�a - Desarrollo Web
(5, 4), -- Laura - Desarrollo Web

(4, 5), -- Pedro - Bases de Datos
(5, 5); -- Laura - Bases de Datos
GO