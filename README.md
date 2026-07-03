# LibraryProject

> Sistema de gestión de biblioteca: API en capas (ASP.NET Core) para el catálogo, préstamos y multas, con un frontend web (Next.js) que consume esa API.

## Tecnologías Utilizadas

**Backend**
* **Framework:** ASP.NET Core 10 (Web API)
* **Persistencia:** PostgreSQL 16
* **ORM:** Entity Framework Core (Code-First)
* **Seguridad:** Autenticación JWT y roles (`Administrador` / `Socio`)
* **Documentación de API:** Swagger / OpenAPI (solo en desarrollo)
* **Infraestructura:** Red privada virtual (Tailscale) y Docker

**Frontend**
* **Framework:** Next.js 16 (App Router, React 19, TypeScript)
* **UI:** Tailwind CSS v4 + shadcn/ui
* **Datos:** SWR para fetch/cache, cliente HTTP propio sobre `fetch` (`frontend/lib/api.ts`)

## Características
* **Registro de usuarios:** Gestión de socios de la biblioteca, cada uno con un número de socio (`NroSocio`) además de su Id interno.
* **Préstamos:** Sistema de salida y entrada de ejemplares, con búsqueda por número de socio en todos los endpoints (no por Id).
* **Cálculo de multas:** Lógica automática para determinar sanciones por devoluciones tardías.
* **Gestión de catálogo:** Alta, baja y consulta de libros.
* **Dashboard y reportes:** Estadísticas agregadas (libros disponibles, préstamos activos, multas pendientes) y reportes de libros más prestados / multas agrupadas por socio.
* **Interfaz web:** Frontend en Next.js que consume toda la API anterior (login/registro, catálogo, préstamos, multas y dashboard).

## Requisitos Previos
* **.NET SDK 10.0** o superior.
* **Node.js 20+** y **npm** (solo si vas a correr o buildear el frontend).
* Conexión a la red **Tailscale** del equipo (ver sección siguiente) o una instancia propia de PostgreSQL 16.

## 🌐 Infraestructura y Conexión (Tailscale)
La base de datos se encuentra centralizada en un servidor Linux. Para poder desarrollar y probar la API localmente contra los datos reales del equipo, es obligatorio estar conectado a la red privada del equipo. Revisá la guía completa en: `docs/tailscale/onboarding.md`

Alternativamente, podés levantar una instancia local de PostgreSQL con `docker compose up postgres` (ver `docker-compose.yml`) y apuntar la cadena de conexión ahí en vez de al servidor de Tailscale.

## 🛠️ Backend: Instalación, Setup Local y Ejecución

Para levantar la API localmente, seguí este orden estricto:

**1. Clonar el repositorio y navegar a la carpeta:**
```bash
git clone https://github.com/AAAActis/LibraryProject.git
cd LibraryProject
```

**2. Restaurar dependencias:**
```bash
dotnet restore
```

**3. Configurar secretos (obligatorio):** la cadena de conexión y la clave JWT no se commitean; se guardan con `dotnet user-secrets`.
```bash
cd Libreria1
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Libreria" "Host=100.125.169.91;Port=5432;Database=libreria;Username=postgres;Password=1234"
dotnet user-secrets set "Jwt:Key" "UnaClaveSuperLargaYSecretaDeAlMenos32Caracteres"
```

**4. Aplicar migraciones (Entity Framework Core):**
```bash
dotnet ef database update
```

**5. Ejecutar la aplicación:**
```bash
dotnet run
```
La API queda disponible en `http://localhost:5000` / `https://localhost:7001`, con Swagger en `/swagger` (solo en `Development`).

## 🖥️ Frontend (Next.js)

El frontend vive en `frontend/` (Next.js 16, App Router) y cubre login/registro, catálogo de libros, usuarios, préstamos, multas y un dashboard con estadísticas. Se integra con la API de dos formas:

**Desarrollo (dos procesos, hot reload):**
```bash
cd frontend
npm install
npm run dev        # http://localhost:3000, pega a http://localhost:5000 (CORS ya habilitado)
```

**Producción (un solo proceso, el backend sirve el frontend como estático):**
```bash
.\build.ps1         # build estático de Next + copia a Libreria1/wwwroot
dotnet run --project Libreria1
# abrir http://localhost:5000 (o el puerto/host donde se despliegue)
```
En este modo el frontend usa rutas relativas (`/api/...`), por lo que no depende de CORS ni de conocer el host en build time.
