-- 1. Creación de la estructura
CREATE TABLE libros (
    isbn VARCHAR(20) PRIMARY KEY,
    titulo VARCHAR(200) NOT NULL,
    autor VARCHAR(100) NOT NULL,
    año_publicacion INTEGER,
    cant_paginas INTEGER,
    esta_disponible BOOLEAN NOT NULL DEFAULT TRUE
);

-- 2. Inserción de datos de prueba
INSERT INTO libros (isbn, titulo, autor, año_publicacion, cant_paginas, esta_disponible) 
VALUES
('978-0132350884', 'Caperucita roja', 'Santi Actis', 2005, 53, TRUE),
('978-8426102146', 'El Libro de la Pastelería', 'Enzo Fernandez', 2010, 250, TRUE),
('978-8437604947', 'Cien años de soledad', 'Gabriel García Márquez', 1967, 496, TRUE),
('978-3161484100', 'Manual de Motores y Bombas Diesel', 'Autor Técnico', 2015, 320, FALSE),
('978-1982181284', 'Steve Jobs: Biografía', 'Walter Isaacson', 2011, 656, TRUE);