# LibraryProject

## Flujo de Trabajo Git (Git Workflow)

Para mantener el código ordenado, evitar conflictos graves y asegurar la calidad del código, este proyecto utiliza un modelo basado en **Feature Branches** y **Pull Requests**. 

### Ramas Principales
* **`main`**: Contiene el código estable y listo para producción. Nadie comitea directamente acá.
* **`develop`**: Es nuestra rama base de integración. Todo el código nuevo se fusiona acá antes de pasar a `main`.

### Guía paso a paso para desarrollar

**1. Sincronizar el entorno local**
Antes de tocar cualquier cosa, asegurate de tener la última versión de la rama de integración.
```bash
git checkout develop
git pull origin develop