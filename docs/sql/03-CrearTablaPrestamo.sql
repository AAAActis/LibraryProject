-- 1. Creación de la estructura
CREATE TABLE prestamos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    libro_isbn VARCHAR(20) NOT NULL REFERENCES libros(isbn) ON DELETE RESTRICT,
    usuario_id UUID NOT NULL REFERENCES usuarios(id) ON DELETE RESTRICT,
    fecha_prestamo TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_devolucion TIMESTAMP,
    esta_activo BOOLEAN NOT NULL DEFAULT TRUE
);
-- 2. Inserción de datos de prueba esto se lo copie al tardiwi
--INSERT INTO prestamos (libro_isbn, usuario_id) VALUES
--('978-3-16-148410-0', '123e4567-e89b-12d3-a456-426614174000'),
--('978-1-4028-9462-6', '123e4567-e89b-12d3-a456-426614174001'),
--('978-0-596-52068-7', '123e4567-e89b-12d3-a456-426614174002');
