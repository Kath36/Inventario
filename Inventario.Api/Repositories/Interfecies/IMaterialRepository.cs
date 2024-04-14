using Inventario.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Api.Repositories.Interfecies
{
    public interface IMaterialRepository
    {
        // Método para guardar un material
        Task<Material> SaveAsync(Material material);
        
        // Método para actualizar un material
        Task<Material> UpdateAsync(Material material);
        
        // Método para obtener todos los materiales
        Task<List<Material>> GetAllAsync();
        
        // Método para eliminar un material por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un material por su ID
        Task<Material> GetById(int id);
    }
}