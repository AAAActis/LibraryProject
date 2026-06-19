using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Libreria1.Domain.Entities;
using Libreria1.Interfaces;

namespace Libreria1.Repositories;

public class RepositorioUsuariosEF : IRepositorio<Usuario, Guid>
{
    private readonly LibreriaContext _context;

    public RepositorioUsuariosEF(LibreriaContext context)
    {
        _context = context;
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        return _context.Usuarios
            .OrderBy(u => u.NroSocio)
            .ToList();
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Id == id);
    }

    public Usuario? BuscarPorEmail(string email)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Email == email);
    }

    public void Agregar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public void Actualizar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        _context.SaveChanges();
    }

    public void Eliminar(Guid id)
    {
        var usuario = ObtenerPorId(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}