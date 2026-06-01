using System;
using System.Linq;
using System.Collections.Generic;
using Libreria1.Interfaces;

public class ServicioUsuario : IUsuarios
{
    private readonly IRepositorio<Usuario> _repositorio;

    public ServicioUsuario(IRepositorio<Usuario> repositorio)
    {
        _repositorio = repositorio;
    }

    public Usuario RegistrarUsuario(string nombre, string email)
    {
        var nuevoUsuario = new Usuario(nombre, email);
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