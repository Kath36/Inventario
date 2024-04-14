using Inventario.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Services.Interfaces
{
    public interface IRegistroMaterialService
    {
        Task<bool> RegistroMaterialExists(int id);
        
        // Método para guardar un registro de material
        Task<RegistroMaterialDto> SaveAsync(RegistroMaterialDto registroMaterialDto);
        
        // Método para actualizar un registro de material
        Task<RegistroMaterialDto> UpdateAsync(RegistroMaterialDto registroMaterialDto);
        
        // Método para obtener todos los registros de materiales
        Task<List<RegistroMaterialDto>> GetAllAsync();
        
        // Método para eliminar un registro de material
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un registro de material por su ID
        Task<RegistroMaterialDto> GetByIdAsync(int id);
    }
}