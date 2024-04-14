using Inventario.Core.Entities;

namespace Inventario.Api.Repositories.Interfecies
{
    public interface IDetallePedidoRepository
    {
        // Método para guardar un detalle de pedido
        Task<DetallePedido> SaveAsync(DetallePedido detallePedido);
        
        // Método para actualizar un detalle de pedido
        Task<DetallePedido> UpdateAsync(DetallePedido detallePedido);
        
        // Método para obtener todos los detalles de pedidos
        Task<List<DetallePedido>> GetAllAsync();
        
        // Método para borrar un detalle de pedido por su ID
        Task<bool> DeleteAsync(int id);
        
        // Método para obtener un detalle de pedido por su ID
        Task<DetallePedido> GetById(int id);
    }
}