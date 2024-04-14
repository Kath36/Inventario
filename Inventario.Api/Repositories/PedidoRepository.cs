using Dapper;
using Dapper.Contrib.Extensions;
using Inventario.Core.Entities;
using Inventario.Api.DataAccess.Interfaces;
using Inventario.Api.Repositories.Interfecies;

namespace Inventario.Api.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IDbContext _dbContext;

        public PedidoRepository(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Pedido> SaveAsync(Pedido pedido)
        {
            pedido.id = await _dbContext.Connection.InsertAsync(pedido);
            return pedido;
        }

        public async Task<Pedido> UpdateAsync(Pedido pedido)
        {
            await _dbContext.Connection.UpdateAsync(pedido);
            return pedido;
        }

        public async Task<List<Pedido>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Pedido WHERE IsDeleted = 0";

            var pedidos = await _dbContext.Connection.QueryAsync<Pedido>(sql);
            return pedidos.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pedido = await GetById(id);
            if (pedido == null)
                return false;

            pedido.IsDeleted = true;

            return await _dbContext.Connection.UpdateAsync(pedido);
        }

        public async Task<Pedido> GetById(int id)
        {
            var pedido = await _dbContext.Connection.GetAsync<Pedido>(id);
            return pedido?.IsDeleted == true ? null : pedido;
        }
    }
}