export interface Usuario {
  id: string
  nroSocio: number
  nombre: string
  apellido: string
  email: string
}

export interface Libro {
  isbn: string
  titulo: string
  autor: string
  anioPublicacion: number
  cantPaginas: number
  estaDisponible: boolean
}

// Respuesta de GET /api/prestamos (PrestamoListadoDto)
export interface Prestamo {
  id: string
  libroIsbn: string
  libroTitulo: string
  nroSocio: number
  usuarioNombre: string
  fechaPrestamo: string
  fechaDevolucion: string | null
  activo: boolean
}

// Respuesta de GET /api/multas (MultaListadoDto)
export interface Multa {
  id: string
  libroTitulo: string
  usuarioNombre: string
  diasRetraso: number
  montoTotal: number
  fechaGenerada: string
}

export interface DashboardStats {
  totalLibros: number
  librosDisponibles: number
  prestamosActivos: number
  multasPendientes: number
}

export interface AuthResponse {
  token: string
}
