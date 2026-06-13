--1. Consulta para obtener los libros más prestados
select I.isbn, I.titulo, I.autor,
    count (p.id) as veces_prestado
from libros I
left join prestamos p on I.isbn = p.libro_isbn
group by I.isbn, I.titulo, I.autor
order by veces_prestado desc
limit 10;

--Resultado obtenido:
--| isbn              | titulo                  | autor               | veces_prestado |
--| ----------------- | ----------------------- | ------------------- | -------------- |
--| 978-0-743-27355-4 | El Gran Gatsby          | F. Scott Fitzgerald | 2              |
--| 978-0-061-96436-9 | El Señor de los Anillos | J.R.R. Tolkien      | 2              |
--| 978-0-062-31609-7 | Sapiens                 | Yuval Noah Harari   | 1              |

--2. Consulta para obtener usuarios con prestamos vencidos
select U.nombre, U.email, P.fecha_prestamo
from usuarios U
join prestamos P on U.id = P.usuario_id
where P.esta_activo = true 
and P.fecha_prestamo < now() - interval '30 days'; --En Claude me pide intervalo, pero lo probe sin pq no habia a mas de 30 dias

--Resultado obtenido:
--| nombre | email                    | fecha_prestamo             |
--| ------ | ------------------------ | -------------------------- |
--| María  | maria.gomez@example.com  | 2026-06-11 02:43:14.343828 |
--| María  | maria.gomez@example.com  | 2026-06-11 02:43:14.343828 |
--| Carlos | carlos.lopez@example.com | 2026-06-11 02:43:14.343828 |
--| Juan   | juan.perez@example.com   | 2026-06-11 02:43:14.343828 |

--3. Consulta para obtener resumen de multas por usuario
select U.nombre, U.apellido, sum (m.monto_total) as total_multas
from usuarios U
inner join prestamos P on U.id = P.usuario_id
inner join multas m on P.id = m.prestamo_id
group by U.nombre, U.apellido
order by total_multas desc;

-- Inserte multas de prueba para obtener resultados en la consulta anterior
INSERT INTO multas (id, prestamo_id, dias_retraso, monto_total, fecha_generada) VALUES
(gen_random_uuid(), '181be75d-5c30-4b4a-b2cf-d857094be70b', 5,  250.00, NOW()),
(gen_random_uuid(), 'a8bac0bb-908a-4aab-91c8-4bd4153094bb', 12, 600.00, NOW()),
(gen_random_uuid(), '3b5f7a8f-5e09-40f3-baf2-c89acdf0fb68', 3,  150.00, NOW()),
(gen_random_uuid(), 'e8b43257-d1c9-4d01-807e-ad1538094003', 20, 1000.00, NOW()),
(gen_random_uuid(), 'd53fd87a-531f-48eb-a260-6df8762f7b55', 7,  350.00, NOW());

--Resultado obtenido:
--| nombre | apellido | total_multas |
--| ------ | -------- | ------------ |
--| Ana    | Martinez | 1000.00      |
--| María  | Gómez    | 850.00       |
--| Juan   | Pérez    | 350.00       |
--| Carlos | López    | 150.00       |