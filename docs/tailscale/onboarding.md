# Guía de Onboarding: Conexión al Servidor Compartido

El proyecto utiliza una base de datos PostgreSQL 16 centralizada, alojada en un servidor Archlinux mediante Docker. Para que los entornos de desarrollo locales puedan comunicarse con este servidor de forma segura, utilizamos Tailscale como red privada virtual (VPN).

Cualquier persona nueva en el equipo debe seguir estos tres pasos para conectar su entorno en menos de 5 minutos.

## 1. Instalar Tailscale
1. Descargar el cliente de Tailscale correspondiente a tu sistema operativo desde la página oficial: [tailscale.com/download](https://tailscale.com/download).
2. Instalar la aplicación y asegurarse de que esté corriendo en segundo plano.

## 2. Unirse a la Red (Invite)
1. Solicitar el enlace de invitación (invite link) al administrador de la infraestructura (Santi).
2. Hacer clic en el enlace e iniciar sesión para vincular tu máquina a la tailnet del proyecto.
3. Abrir la aplicación de Tailscale y verificar que el estado indique **"Connected"**.

## 3. Configurar la Cadena de Conexión
El motor de base de datos responde exclusivamente a través de la IP de la VPN. Es necesario apuntar el entorno de ejecución local hacia esa dirección.

1. Abrir el archivo `appsettings.Development.json` (o `appsettings.json`) en la raíz del proyecto API.
2. Modificar el bloque de `ConnectionStrings` para que la clave `"Libreria"` apunte a la IP `100.125.169.91` por el puerto `5432`:

```json
"ConnectionStrings": {
  "Libreria": "Host=100.125.169.91;Port=5432;Database=libreria;Username=postgres;Password=SOLICITAR_POR_PRIVADO"
}
```
> **Importante:** La contraseña del usuario `postgres` debe solicitarse por canal privado. Por motivos de seguridad, nunca se debe commitear la contraseña real en el repositorio.

## 4. Validar la Conexión
Para confirmar que el enrutamiento funciona correctamente, abrí una terminal en la carpeta del proyecto y ejecutá:
```bash
dotnet ef database update
```
Si el proceso finaliza sin errores (`Build succeeded`), tu máquina ya tiene acceso de lectura/escritura a la base de datos compartida.