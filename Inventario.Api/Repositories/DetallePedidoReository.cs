using Dapper;
using Dapper.Contrib.Extensions;
using Inventario.Core.Entities;
using Inventario.Api.DataAccess.Interfaces;
using Inventario.Api.Repositories.Interfecies;

namespace Inventario.Api.Repositories
{
    public class DetallePedidoRepository : IDetallePedidoRepository
    {
        private readonly IDbContext _dbContext;

        public DetallePedidoRepository(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task<DetallePedido> SaveAsync(DetallePedido detallePedido)
        {
            detallePedido.id = await _dbContext.Connection.InsertAsync(detallePedido);
            return detallePedido;
        }

        public async Task<DetallePedido> UpdateAsync(DetallePedido detallePedido)
        {
            await _dbContext.Connection.UpdateAsync(detallePedido);
            return detallePedido;
        }

        public async Task<List<DetallePedido>> GetAllAsync()
        {
            const string sql = "SELECT * FROM DetallePedido WHERE IsDeleted = 0";

            var detallesPedidos = await _dbContext.Connection.QueryAsync<DetallePedido>(sql);
            return detallesPedidos.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var detallePedido = await GetById(id);
            if (detallePedido == null)
                return false;

            detallePedido.IsDeleted = true;

            return await _dbContext.Connection.UpdateAsync(detallePedido);
        }

        public async Task<DetallePedido> GetById(int id)
        {
            var detallePedido = await _dbContext.Connection.GetAsync<DetallePedido>(id);
            return detallePedido?.IsDeleted == true ? null : detallePedido;
        }
    }
}