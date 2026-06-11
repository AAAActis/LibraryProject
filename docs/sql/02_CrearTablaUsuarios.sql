--1 Creación de la estructura
CREATE TABLE usuarios (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nro_socio SERIAL NOT NULL UNIQUE,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    email VARCHAR(200) NOT NULL UNIQUE,
    telefono VARCHAR(20),
    documento VARCHAR(20) NOT NULL UNIQUE,
    fecha_registro TIMESTAMP NOT NULL DEFAULT now()
);

-- 2. Inserción de datos de prueba
INSERT INTO usuarios (nro_socio, nombre, apellido, email, telefono, documento)
VALUES
(1, 'Juan', 'Pérez', 'juan.perez@example.com', '123456789', '12345678'),
(2, 'María', 'Gómez', 'maria.gomez@example.com', '987654321', '87654321'),
(3, 'Carlos', 'López', 'carlos.lopez@example.com', '555555555', '55555555');

--3. Consulta de prueba
SELECT * FROM usuarios;

-- Resultado obtenido:
--| id                                   | nro_socio | nombre | apellido | email                    | telefono  | documento | fecha_registro             |
--| ------------------------------------ | --------- | ------ | -------- | ------------------------ | --------- | --------- | -------------------------- |
--| fe27029f-4ddc-4994-89a1-9f34c780a90c | 1         | Juan   | Pérez    | juan.perez@example.com   | 123456789 | 12345678  | 2026-06-11 00:29:00.996137 |
--| a515b72b-ab6a-41d5-97cd-7ed60d09a4cb | 2         | María  | Gómez    | maria.gomez@example.com  | 987654321 | 87654321  | 2026-06-11 00:29:00.996137 |
--| 798eae83-d206-46a1-b278-997e9de40fd1 | 3         | Carlos | López    | carlos.lopez@example.com | 555555555 | 55555555  | 2026-06-11 00:29:00.996137 |

--4. Consulta de prueba dos usuarios con el mismo email y nro_socio (debería fallar por la restricción UNIQUE)
INSERT INTO usuarios (nro_socio, nombre, apellido, email, telefono, documento)
VALUES
(1, 'Juan', 'Pérez', 'juan.perez@example.com', '123456789', '12345678'),
(2, 'María', 'Gómez', 'maria.gomez@example.com', '987654321', '87654321'),
(3, 'Carlos', 'López', 'carlos.lopez@example.com', '555555555', '55555555'),
(4, 'Ana', 'Martinez', 'juan.perez@example.com', '46883773', '3516131380');
--Resultado obtenido:
--ERROR: 23505: duplicate key value violates unique constraint "usuarios_nro_socio_key"
--DETAIL: Key (nro_socio)=(1) already exists.

--5 Actualización de un numero de teléfono de un usuario
UPDATE usuarios 
SET telefono = '111222333'
WHERE id = 'fe27029f-4ddc-4994-89a1-9f34c780a90c';
-- Resultado obtenido:
--Success. No rows returned.
