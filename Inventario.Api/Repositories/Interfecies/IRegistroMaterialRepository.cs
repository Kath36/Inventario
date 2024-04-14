using Inventario.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Api.Repositories.Interfecies
{
    public interface IRegistroMaterialRepository
    {
        // Método para guardar un registro de material
        Task<RegistroMaterial> SaveAsync(RegistroMaterial registroMaterial);
        
        // Método para actualizar un registro de material
        Task<RegistroMaterial> UpdateAsync(RegistroMaterial registroMaterial);
        
        // Método para obtener todos los registros de material
        Task<List<RegistroMaterial>> GetAllAsync();
        
        // Método para eliminar un registro de material por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un registro de material por su ID
        Task<RegistroMaterial> GetById(int id);
    }
}