using Inventario.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Api.Repositories.Interfecies
{
    public interface IProveedorRepository
    {
        // Método para guardar un proveedor
        Task<Proveedor> SaveAsync(Proveedor proveedor);
        
        // Método para actualizar un proveedor
        Task<Proveedor> UpdateAsync(Proveedor proveedor);
        
        // Método para obtener todos los proveedores
        Task<List<Proveedor>> GetAllAsync();
        
        // Método para eliminar un proveedor por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un proveedor por su ID
        Task<Proveedor> GetById(int id);
    }
}