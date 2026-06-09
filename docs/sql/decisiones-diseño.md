# Decisiones de diseño — Base de datos Librería

## ¿Por qué UUID y no SERIAL para los IDs?
UUID es consistente con el modelo de dominio en C# (Guid). 
Permite generar el ID en la aplicación sin consultar la DB.

## ¿Por qué email tiene constraint UNIQUE?
Un usuario no puede registrarse dos veces con el mismo email.
Es una regla de negocio del dominio.

## ¿Qué pasa si se elimina un libro con préstamos?
ON DELETE RESTRICT — no se puede eliminar un libro 
que tiene préstamos asociados. Protege la integridad de los datos.