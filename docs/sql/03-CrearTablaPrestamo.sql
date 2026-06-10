-- 1. Creación de la estructura
CREATE TABLE prestamos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    libro_isbn VARCHAR(20) NOT NULL REFERENCES libros(isbn),
    usuario_id UUID NOT NULL REFERENCES usuarios(id),
    fecha_prestamo TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_devolucion TIMESTAMP (nullable — sin NOT NULL),
    esta_activo BOOLEAN NOT NULL DEFAULT TRUE
    ON DELETE CASCADE -- CHICOS NO SE Q ES TO HAY Q DEBATIRLO PERO NI IDEA ES ENTRE CASCADE Y RESTRICT

);
-- 2. Inserción de datos de prueba esto se lo copie al tardiwi
--INSERT INTO prestamos (libro_isbn, usuario_id) VALUES
--('978-3-16-148410-0', '123e4567-e89b-12d3-a456-426614174000'),
--('978-1-4028-9462-6', '123e4567-e89b-12d3-a456-426614174001'),
--('978-0-596-52068-7', '123e4567-e89b-12d3-a456-426614174002');
