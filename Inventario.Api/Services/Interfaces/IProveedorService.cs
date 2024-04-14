using Inventario.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Services.Interfaces
{
    public interface IProveedorService
    {
        Task<bool> ProveedorExists(int id);
        
        // Método para guardar un proveedor
        Task<ProveedorDto> SaveAsync(ProveedorDto proveedorDto);
        
        // Método para actualizar un proveedor
        Task<ProveedorDto> UpdateAsync(ProveedorDto proveedorDto);
        
        // Método para obtener todos los proveedores
        Task<List<ProveedorDto>> GetAllAsync();
        
        // Método para eliminar un proveedor
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un proveedor por su ID
        Task<ProveedorDto> GetByIdAsync(int id);
    }
}