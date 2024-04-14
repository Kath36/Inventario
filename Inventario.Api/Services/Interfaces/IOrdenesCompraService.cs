using Inventario.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario.Services.Interfaces
{
    public interface IOrdenCompraService
    {
        Task<bool> OrdenCompraExists(int id);
        
        // Método para guardar una orden de compra
        Task<OrdenCompraDto> SaveAsync(OrdenCompraDto ordenCompra);
        
        // Método para actualizar una orden de compra
        Task<OrdenCompraDto> UpdateAsync(OrdenCompraDto ordenCompra);
        
        // Método para obtener todas las órdenes de compra
        Task<List<OrdenCompraDto>> GetAllAsync();
        
        // Método para eliminar una orden de compra por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener una orden de compra por su ID
        Task<OrdenCompraDto> GetById(int id);
    }
}