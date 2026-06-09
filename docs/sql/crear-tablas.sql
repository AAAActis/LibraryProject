CREATE TABLE "libros" (
  "isbn" varchar(20) PRIMARY KEY,
  "titulo" varchar(255) NOT NULL,
  "autor" varchar(255) NOT NULL,
  "ano_publicacion" integer,
  "cant_paginas" integer,
  "esta_disponible" boolean DEFAULT true
);

CREATE TABLE "usuarios" (
  "id" uuid PRIMARY KEY,
  "nro_socio" integer UNIQUE NOT NULL,
  "nombre" varchar(100) NOT NULL,
  "apellido" varchar(100) NOT NULL,
  "email" varchar(255) UNIQUE NOT NULL,
  "telefono" varchar(20),
  "documento" varchar(20),
  "fecha_registro" timestamp DEFAULT (now())
);

CREATE TABLE "prestamos" (
  "id" uuid PRIMARY KEY,
  "libro_isbn" integer NOT NULL,
  "usuario_id" uuid NOT NULL,
  "fecha_prestamo" timestamp NOT NULL DEFAULT (now()),
  "fecha_devolucion" timestamp,
  "esta_activo" boolean DEFAULT true
);

CREATE TABLE "multas" (
  "id" uuid PRIMARY KEY,
  "prestamo_id" uuid NOT NULL,
  "dias_retraso" integer NOT NULL,
  "monto_total" decimal(10,2) NOT NULL,
  "fecha_generada" timestamp DEFAULT (now())
);

ALTER TABLE "prestamos" ADD FOREIGN KEY ("libro_isbn") REFERENCES "libros" ("isbn") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "prestamos" ADD FOREIGN KEY ("usuario_id") REFERENCES "usuarios" ("id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "multas" ADD FOREIGN KEY ("prestamo_id") REFERENCES "prestamos" ("id") DEFERRABLE INITIALLY IMMEDIATE;
