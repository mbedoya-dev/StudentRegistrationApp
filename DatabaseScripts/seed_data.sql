-- Insertar las 10 materias iniciales
INSERT INTO subjects (subject_name) VALUES
('Matem�ticas I'),
('F�sica I'),
('Qu�mica I'),
('Fundamentos de Ciencias de la Computaci�n'),
('Fundamentos de Desarrollo Web'),
('Gesti�n de Bases de Datos'),
('Algoritmos y Estructuras de Datos'),
('Programaci�n Orientada a Objetos'),
('Ingenier�a de Software'),
('Gesti�n de Proyectos');
GO

-- Insertar los 5 profesores iniciales
INSERT INTO professors (first_name, last_name, email) VALUES
('Ana', 'Garc�a', 'ana.garcia@uni.com'),
('Juan', 'Rodr�guez', 'juan.rodriguez@uni.com'),
('Mar�a', 'Fern�ndez', 'maria.fernandez@uni.com'),
('Pedro', 'L�pez', 'pedro.lopez@uni.com'),
('Laura', 'Mart�nez', 'laura.martinez@uni.com');
GO

-- Asignar 2 materias a cada profesor
INSERT INTO professor_subjects (professor_id, subject_id) VALUES
(1, 1), (1, 2), -- Ana Garc�a dicta Matem�ticas I, F�sica I
(2, 3), (2, 4), -- Juan Rodr�guez dicta Qu�mica I, Fundamentos de Ciencias de la Computaci�n
(3, 5), (3, 6), -- Mar�a Fern�ndez dicta Fundamentos de Desarrollo Web, Gesti�n de Bases de Datos
(4, 7), (4, 8), -- Pedro L�pez dicta Algoritmos y Estructuras de Datos, Programaci�n Orientada a Objetos
(5, 9), (5, 10); -- Laura Mart�nez dicta Ingenier�a de Software, Gesti�n de Proyectos
GO

