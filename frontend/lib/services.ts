import { api, setToken, clearToken } from "@/lib/api"
import type {
  AuthResponse,
  DashboardStats,
  Libro,
  Multa,
  Prestamo,
  Usuario,
} from "@/lib/types"

// --- Auth ---
export async function login(email: string, password: string) {
  const res = await api.post<AuthResponse>(
    "/api/auth/login",
    { email, password },
    { auth: false },
  )
  if (res?.token) setToken(res.token)
  return res
}

// El registro es la creación de un usuario (POST /api/usuarios); no existe un
// endpoint separado de "auth/register". CrearUsuarioDto exige teléfono y
// documento además de nombre/apellido/email/password.
export async function register(input: {
  nombre: string
  apellido: string
  email: string
  telefono: string
  documento: string
  password: string
}) {
  return api.post<Usuario>("/api/usuarios", input, { auth: false })
}

export function logout() {
  clearToken()
}

// --- Dashboard ---
export function getStats() {
  return api.get<DashboardStats>("/api/dashboard/stats")
}

// --- Libros ---
export function getLibros() {
  return api.get<Libro[]>("/api/libros")
}

export function createLibro(input: {
  isbn: string
  titulo: string
  autor: string
  anioPublicacion: number
  cantPaginas: number
}) {
  return api.post<Libro>("/api/libros", input)
}

// --- Usuarios ---
export function getUsuarios() {
  return api.get<Usuario[]>("/api/usuarios")
}

// --- Préstamos ---
export function getPrestamos() {
  return api.get<Prestamo[]>("/api/prestamos")
}

export function createPrestamo(input: { isbn: string; nroSocio: number }) {
  return api.post<{ mensaje: string }>("/api/prestamos", {
    libroIsbn: input.isbn,
    nroSocio: input.nroSocio,
  })
}

// PUT /api/prestamos/devolver  (body, no id en la URL). Si el préstamo
// vuelve con retraso, el backend incluye la multa generada en la respuesta.
export function devolverPrestamo(input: { libroIsbn: string; nroSocio: number }) {
  return api.put<{ prestamo: unknown; multa?: Multa }>("/api/prestamos/devolver", {
    libroIsbn: input.libroIsbn,
    usuarioId: input.nroSocio,
  })
}

// --- Multas ---
export function getMultas() {
  return api.get<Multa[]>("/api/multas")
}
