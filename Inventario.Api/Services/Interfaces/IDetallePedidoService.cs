using Inventario.Api.Dto;

namespace Inventario.Services.Interfaces
{
    public interface IDetallePedidoService
    {
        Task<bool> DetallePedidoExists(int id);
        
        // Método para guardar detalles de pedido
        Task<DetallePedidoDto> SaveAsync(DetallePedidoDto detallePedido);
        
        // Método para actualizar detalles de pedido
        Task<DetallePedidoDto> UpdateAsync(DetallePedidoDto detallePedido);
        
        // Método para obtener todos los detalles de pedidos
        Task<List<DetallePedidoDto>> GetAllAsync();
        
        // Método para borrar un detalle de pedido por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un detalle de pedido por su ID
        Task<DetallePedidoDto> GetById(int id);
    }
}