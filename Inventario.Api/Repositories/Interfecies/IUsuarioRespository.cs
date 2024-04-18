using Inventario.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Api.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        // Método para guardar un usuario
        Task<Usuario> AddAsync(Usuario usuario);

        // Método para actualizar un usuario
        Task<bool> UpdateAsync(Usuario usuario);

        // Método para obtener todos los usuarios
        Task<List<Usuario>> GetAllAsync();

        // Método para obtener un usuario por su ID
        Task<Usuario> GetByIdAsync(int id);

        // Método para eliminar un usuario por su ID
        Task<bool> DeleteAsync(int id);

        // Método para verificar si un usuario existe por su ID
        Task<bool> UsuarioExistsByIdAsync(int id);

        // Método para verificar si un usuario existe por su correo electrónico
        Task<bool> UsuarioExistsByEmailAsync(string email);
        
        // Método para obtener un usuario por su correo electrónico
        Task<Usuario> GetByEmailAsync(string email);
    }
}