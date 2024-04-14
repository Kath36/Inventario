using Inventario.Api.Dto;

namespace Inventario.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<bool> PedidoExists(int id);
        
        // Método para guardar pedidos
        Task<PedidoDto> SaveAsync(PedidoDto pedido);
        
        // Método para actualizar pedidos
        Task<PedidoDto> UpdateAsync(PedidoDto pedido);
        
        // Método para obtener todos los pedidos
        Task<List<PedidoDto>> GetAllAsync();
        
        // Método para borrar un pedido por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un pedido por su ID
        Task<PedidoDto> GetById(int id);
    }
}