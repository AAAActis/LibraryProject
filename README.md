## LibraryProject

> Aplicación de consola para la gestión de una biblioteca, enfocada en el manejo eficiente del catálogo y administración de préstamos.

## Tecnologías Utilizadas
* **Lenguaje:** C#
* **Framework:** .NET 10
* **Persistencia:** Basada en **Interfaces** (In-memory storage). El sistema está diseñado para ser extensible, permitiendo acoplar una base de datos real en el futuro sin afectar la lógica de negocio.

## Características
* **Registro de usuarios:** Gestión de socios de la biblioteca.
* **Préstamos:** Sistema de salida y entrada de ejemplares.
* **Cálculo de multas:** Lógica automática para determinar sanciones por devoluciones tardías.
* **Gestión de Catálogo:** Alta, baja y consulta de libros.

## Requisitos Previos
* **.NET SDK 10.0** o superior.

## Instalación y Ejecución

1. **Clona el repositorio:**
   ```bash
   git clone [https://github.com/AAAActis/LibraryProject.git](https://github.com/AAAActis/LibraryProject.git)
2. **Navega al directorio del proyecto**
   ```bash
   cd LibraryProject 
3. **Restaurar dependencias**
   ```bash
   dotnet restore 
4. **Ejecturar la aplicacion**
   ```bash
   dotnet run