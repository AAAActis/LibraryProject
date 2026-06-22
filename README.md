# LibraryProject

> API RESTful estructurada en capas para la gestión de una biblioteca, enfocada en el manejo seguro del catálogo, administración de préstamos y cálculo automatizado de multas.

## Tecnologías Utilizadas
* **Framework:** ASP.NET Core 10 (Web API)
* **Persistencia:** PostgreSQL 16
* **ORM:** Entity Framework Core (Code-First)
* **Seguridad:** Autenticación JWT y roles
* **Infraestructura:** Red privada virtual (Tailscale) y Docker

## Características
* **Registro de usuarios:** Gestión de socios de la biblioteca.
* **Préstamos:** Sistema de salida y entrada de ejemplares.
* **Cálculo de multas:** Lógica automática para determinar sanciones por devoluciones tardías.
* **Gestión de Catálogo:** Alta, baja y consulta de libros.

## Requisitos Previos
* **.NET SDK 10.0** o superior.

## 🌐 Infraestructura y Conexión (Tailscale)
La base de datos se encuentra centralizada en un servidor Linux. Para poder desarrollar y probar la API localmente, es obligatorio estar conectado a la red privada del equipo. Revisá la guía completa en: `docs/tailscale/onboarding.md`

## 🛠️ Instalación, Setup Local y Ejecución

Para levantar la API localmente, seguí este orden estricto:

**1. Clonar el repositorio y navegar a la carpeta:**
```bash
git clone [https://github.com/AAAActis/LibraryProject.git](https://github.com/AAAActis/LibraryProject.git)
cd LibraryProject
2. **Restaurar dependencias:**
   dotnet restore
3. **Configurar la Base de Datos (Obligatorio)**
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:Libreria" "Host=100.125.169.91;Port=5432;Database=libreria;Username=postgres;Password=1234"
4. **Aplicar Migraciones (Entity Framework Core)**
   dotnet ef database update
5. **Ejecutar la Aplicación:**
   dotnet run