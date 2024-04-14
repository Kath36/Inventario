using Inventario.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<bool> MaterialExists(int id);

        // Método para guardar un material
        Task<MaterialDto> SaveAsync(MaterialDto material);

        // Método para actualizar un material
        Task<MaterialDto> UpdateAsync(MaterialDto material);

        // Método para obtener todos los materiales
        Task<List<MaterialDto>> GetAllAsync();

        // Método para eliminar un material por su ID
        Task<bool> DeleteAsync(int id);

        // Método para obtener un material por su ID
        Task<MaterialDto> GetByIdAsync(int id);
    }
}