# Setup de Infraestructura: Tailscale en el Servidor

El servidor principal (Archlinux) utiliza Tailscale para exponer el motor de PostgreSQL a la red privada del equipo. Al ser una máquina de infraestructura desatendida, es crítico configurar el nodo para que la conexión sea permanente.

## Deshabilitar Key Expiry (Vencimiento de Clave)

Por defecto, Tailscale exige reautenticar los dispositivos cada 180 días. Si esto ocurre en el servidor, la base de datos quedará offline y bloqueará el desarrollo de todo el equipo.

**Procedimiento:**
1. Ingresar a la consola de administración con credenciales de administrador: [login.tailscale.com/admin/machines](https://login.tailscale.com/admin/machines).
2. Localizar en la lista la máquina correspondiente al servidor (hostname `ArchLinux`).
3. Hacer clic en el menú de opciones (los tres puntos `...`) en el extremo derecho de la fila del dispositivo.
4. Seleccionar la opción **Disable key expiry**.
5. Validar el cambio confirmando que el dispositivo ahora muestra la etiqueta visual `Expiry disabled`.