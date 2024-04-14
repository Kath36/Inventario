using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario.Core.Entities;

namespace Inventario.Api.Repositories.Interfecies
{
    public interface IOrdenCompraRepository
    {
        // Método para guardar una orden de compra
        Task<OrdenCompra> SaveAsync(OrdenCompra ordenCompra);
        
        // Método para actualizar una orden de compra
        Task<OrdenCompra> UpdateAsync(OrdenCompra ordenCompra);
        
        // Método para obtener todas las órdenes de compra
        Task<List<OrdenCompra>> GetAllAsync();
        
        // Método para eliminar una orden de compra por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener una orden de compra por su ID
        Task<OrdenCompra> GetById(int id);
    }
}