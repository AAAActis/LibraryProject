using System;
using System.Linq;
using System.Collections.Generic;
using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;
using Libreria1.Domain.Exceptions;

namespace Libreria1.Application.Services
{
    public class ServicioUsuario : IUsuarios
{
    private readonly IRepositorio<Usuario, Guid> _repositorio;

    public ServicioUsuario(IRepositorio<Usuario, Guid> repositorio)
    {
        _repositorio = repositorio;
    }

    public Usuario RegistrarUsuario(string nombre, string apellido, string email, string password)
    {
        //verificar si email existe
        var existeEmail = _repositorio.ObtenerTodos()
            .Any(u => u.Email == email);
        if (existeEmail)
        {
            throw new UsuarioYaExisteException($"El email {email} ya está registrado.");
        }
        var nuevoUsuario = new Usuario(nombre, apellido, email, password);
        _repositorio.Agregar(nuevoUsuario);
        return nuevoUsuario;
    }

    public Usuario BuscarPorNumeroSocio(int nroSocio)
    {
        var usuario = _repositorio.ObtenerTodos().FirstOrDefault(u => u.NroSocio == nroSocio);
        if (usuario == null)
        {
            throw new UsuarioNoEncontradoException($"Usuario con número de socio {nroSocio} no encontrado.");
        }
        return usuario;
    }

    public Usuario? BuscarPorEmail(string email)
        {
            return _repositorio.ObtenerTodos().FirstOrDefault(u => u.Email == email);
        }

    public Usuario? BuscarPorId(Guid id)
    {
        return _repositorio.ObtenerPorId(id);
    }

    public List<Usuario> ListarUsuarios()
    {
        return _repositorio.ObtenerTodos().ToList();
    }

    public bool EliminarUsuario(Guid id)
    {
        var existente = _repositorio.ObtenerPorId(id);
        if (existente == null) return false;
        _repositorio.Eliminar(id);
        return true;
    }
}
}