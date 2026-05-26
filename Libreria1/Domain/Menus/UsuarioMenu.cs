using System;
using Libreria1.Application;
using Libreria1.Services;

namespace Libreria1.Domain.Menus
{
    public class UsuarioMenu : MenuBase
    {
        private readonly ServicioUsuario _servicioUsuario;

        public UsuarioMenu(ServicioUsuario servicioUsuario, IPresenter presenter)
            : base(presenter)
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
                Console.WriteLine("2. Buscar usuario por ID");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("Opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RegistrarUsuario();
                        break;
                    case "2":
                        BuscarUsuarioPorId();
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

            _servicioUsuario.AgregarUsuario(new Usuario(nombreUsuario ?? string.Empty, emailUsuario ?? string.Empty));
            MostrarMensaje("Usuario registrado exitosamente.");
        }

        private void BuscarUsuarioPorId()
        {
            Console.Write("Ingrese el ID del usuario a buscar: ");
            string? idUsuario = Console.ReadLine();

            if (!Guid.TryParse(idUsuario, out Guid id))
            {
                MostrarMensaje("ID inválido.");
                return;
            }

            try
            {
                Usuario usuarioEncontrado = _servicioUsuario.BuscarUsuario(id);
                MostrarMensaje($"Usuario encontrado: {usuarioEncontrado.nombre} ({usuarioEncontrado.email})");
            }
            catch (UsuarioNoEncontradoException)
            {
                MostrarMensaje("Usuario no encontrado.");
            }
        }
    }
}
