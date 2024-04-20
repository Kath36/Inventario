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
            try
            {
                detallePedido.id = await _dbContext.Connection.InsertAsync(detallePedido);
                return detallePedido;
            }
            catch (Exception ex)
            {
                // Manejar la excepción o lanzarla nuevamente si se quiere propagar hacia arriba.
                throw new Exception("Repository", ex);
            }
        }

        public async Task<DetallePedido> UpdateAsync(DetallePedido detallePedido)
        {
            try
            {
                await _dbContext.Connection.UpdateAsync(detallePedido);
                return detallePedido;
            }
            catch (Exception ex)
            {
                throw new Exception("Repository", ex);
            }
        }

        public async Task<List<DetallePedido>> GetAllAsync()
        {
            try
            {
                const string sql = "SELECT * FROM DetallePedido WHERE IsDeleted = 0";
                var detallesPedidos = await _dbContext.Connection.QueryAsync<DetallePedido>(sql);
                return detallesPedidos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Repository", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var detallePedido = await GetById(id);
                if (detallePedido == null)
                    return false;

                detallePedido.IsDeleted = true;

                return await _dbContext.Connection.UpdateAsync(detallePedido);
            }
            catch (Exception ex)
            {
                throw new Exception("Repository", ex);
            }
        }

        public async Task<DetallePedido> GetById(int id)
        {
            try
            {
                var detallePedido = await _dbContext.Connection.GetAsync<DetallePedido>(id);
                return detallePedido?.IsDeleted == true ? null : detallePedido;
            }
            catch (Exception ex)
            {
                throw new Exception("Repository", ex);
            }
        }
    }
}
