using System;
using Libreria1.Interfaces;

namespace Libreria1.Domain.Menus
{
    public class UsuarioMenu : MenuBase
    {
        private readonly IUsuarios _servicioUsuario;

        public UsuarioMenu(IUsuarios servicioUsuario)
        {
            _servicioUsuario = servicioUsuario;
        }

        public override void Ejecutar()
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                MostrarTitulo("Menú de Usuarios");
                Console.WriteLine("1. Registrar usuario");
                Console.WriteLine("2. Buscar usuario por número de socio");
                Console.WriteLine("3. Eliminar usuario por número de socio");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("Opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RegistrarUsuario();
                        break;
                    case "2":
                        BuscarUsuarioPorNumeroSocio();
                        break;
                    case "3":
                        EliminarUsuarioPorNumeroSocio();
                        break;
                    case "0":
                        volver = true;
                        break;
                    default:
                        MostrarMensaje("Opción no válida.");
                        break;
                }

                if (!volver)
                {
                    EsperarContinuar();
                }
            }
        }

        private void RegistrarUsuario()
        {
            Console.Write("Ingrese el nombre del usuario: ");
            string? nombreUsuario = Console.ReadLine();

            Console.Write("Ingrese el email del usuario: ");
            string? emailUsuario = Console.ReadLine();

            var creado = _servicioUsuario.RegistrarUsuario(nombreUsuario ?? string.Empty, emailUsuario ?? string.Empty);
            MostrarMensaje($"Usuario creado con numero de socio: {creado.NroSocio}");
        }

        private void BuscarUsuarioPorNumeroSocio()
        {
            Console.Write("Ingrese el número de socio del usuario a buscar: ");
            string? nro = Console.ReadLine();

            if (!int.TryParse(nro, out int nroSocio))
            {
                MostrarMensaje("Número de socio inválido.");
                return;
            }

            try
            {
                Usuario usuarioEncontrado = _servicioUsuario.BuscarPorNumeroSocio(nroSocio);
                MostrarMensaje($"Usuario encontrado: {usuarioEncontrado.Nombre} ({usuarioEncontrado.Email}) - Id: {usuarioEncontrado.Id}");
            }
            catch (UsuarioNoEncontradoException ex)
            {
                MostrarMensaje(ex.Message);
            }
        }

        private void EliminarUsuarioPorNumeroSocio()
        {
            Console.Write("Ingrese el número de socio del usuario a eliminar: ");
            string? nro = Console.ReadLine();

            if (!int.TryParse(nro, out int nroSocio))
            {
                MostrarMensaje("Número de socio inválido.");
                return;
            }

            try
            {
                var usuario = _servicioUsuario.BuscarPorNumeroSocio(nroSocio);
                var eliminado = _servicioUsuario.EliminarUsuario(usuario.Id);
                if (eliminado)
                {
                    MostrarMensaje($"Usuario eliminado con Id {usuario.Id} y número de socio {usuario.NroSocio}");
                }
                else
                {
                    MostrarMensaje("Error al eliminar el usuario.");
                }
            }
            catch (UsuarioNoEncontradoException ex)
            {
                MostrarMensaje(ex.Message);
            }
        }
    }
}
