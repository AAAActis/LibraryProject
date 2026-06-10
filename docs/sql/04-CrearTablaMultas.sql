CREATE TABLE multas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    prestamo_id UUID NOT NULL REFERENCES prestamos(id),
    dias_retraso INTEGER NOT NULL,
    monto_total NUMERIC(10,2) NOT NULL,
    fecha_generada TIMESTAMP NOT NULL DEFAULT NOW()
);