// Central API client for the LibraryProject backend.
//
// NEXT_PUBLIC_API_URL is baked in at build time:
//  - sin setear (dev, "npm run dev")        -> apunta a https://localhost:7001
//  - vacío (build de producción, ver build.ps1) -> requests relativas ("/api/...").
//    Esto es lo que se usa cuando el frontend se sirve desde el propio backend
//    (mismo origen), sin necesidad de CORS ni de conocer el host en build time.
//
// En dev se apunta directo a HTTPS (7001) y no a HTTP (5000): el backend fuerza
// UseHttpsRedirection antes de UseCors, así que un preflight OPTIONS a :5000
// recibe un 307 en vez de las cabeceras CORS, y el navegador lo trata como un
// fallo de red ("No se pudo conectar con el servidor"). Requiere haber corrido
// una vez `dotnet dev-certs https --trust`.
//
// Para authenticated endpoints el JWT se lee de localStorage bajo la key
// "token" y se manda como header `Authorization: Bearer <token>`.

export const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? "https://localhost:7001"

export const TOKEN_KEY = "token"

export function getToken(): string | null {
  if (typeof window === "undefined") return null
  return window.localStorage.getItem(TOKEN_KEY)
}

export function setToken(token: string): void {
  if (typeof window === "undefined") return
  window.localStorage.setItem(TOKEN_KEY, token)
}

export function clearToken(): void {
  if (typeof window === "undefined") return
  window.localStorage.removeItem(TOKEN_KEY)
}

export function isAuthenticated(): boolean {
  return !!getToken()
}

// Error thrown by the API layer. Carries the HTTP status so callers can
// branch on specific codes (e.g. 400 vs 404).
export class ApiError extends Error {
  status: number
  data: unknown

  constructor(message: string, status: number, data?: unknown) {
    super(message)
    this.name = "ApiError"
    this.status = status
    this.data = data
  }
}

type RequestOptions = Omit<RequestInit, "body"> & {
  // When true (default), attach the Authorization header from localStorage.
  auth?: boolean
  body?: unknown
}

async function request<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const { auth = true, body, headers, ...rest } = options

  const finalHeaders: Record<string, string> = {
    Accept: "application/json",
    ...(headers as Record<string, string>),
  }

  if (body !== undefined) {
    finalHeaders["Content-Type"] = "application/json"
  }

  if (auth) {
    const token = getToken()
    if (token) {
      finalHeaders["Authorization"] = `Bearer ${token}`
    }
  }

  let res: Response
  try {
    res = await fetch(`${API_BASE_URL}${path}`, {
      ...rest,
      headers: finalHeaders,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    })
  } catch {
    throw new ApiError(
      "No se pudo conectar con el servidor. Verifica que el backend esté en ejecución.",
      0,
    )
  }

  // Attempt to parse a JSON payload if present.
  const text = await res.text()
  let data: unknown = null
  if (text) {
    try {
      data = JSON.parse(text)
    } catch {
      data = text
    }
  }

  if (!res.ok) {
    const message =
      (data && typeof data === "object" && "message" in data
        ? String((data as { message: unknown }).message)
        : typeof data === "string" && data
          ? data
          : `Error ${res.status}`) || `Error ${res.status}`
    throw new ApiError(message, res.status, data)
  }

  return data as T
}

export const api = {
  get: <T>(path: string, options?: RequestOptions) =>
    request<T>(path, { ...options, method: "GET" }),
  post: <T>(path: string, body?: unknown, options?: RequestOptions) =>
    request<T>(path, { ...options, method: "POST", body }),
  put: <T>(path: string, body?: unknown, options?: RequestOptions) =>
    request<T>(path, { ...options, method: "PUT", body }),
  patch: <T>(path: string, body?: unknown, options?: RequestOptions) =>
    request<T>(path, { ...options, method: "PATCH", body }),
  delete: <T>(path: string, options?: RequestOptions) =>
    request<T>(path, { ...options, method: "DELETE" }),
}
