using Inventario.Core.Entities;

namespace Inventario.Api.Repositories.Interfecies
{
    public interface IPedidoRepository
    {
        // Método para guardar un pedido
        Task<Pedido> SaveAsync(Pedido pedido);
        
        // Método para actualizar un pedido
        Task<Pedido> UpdateAsync(Pedido pedido);
        
        // Método para obtener todos los pedidos
        Task<List<Pedido>> GetAllAsync();
        
        // Método para borrar un pedido por su ID
        Task<bool> DeleteAsync(int ID);
        
        // Método para obtener un pedido por su ID
        Task<Pedido> GetById(int ID);
    }
}