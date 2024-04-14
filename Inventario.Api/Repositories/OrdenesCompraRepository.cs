using Dapper;
using Dapper.Contrib.Extensions;
using Inventario.Core.Entities;
using Inventario.Api.DataAccess.Interfaces;
using Inventario.Api.Repositories.Interfecies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario.Api.Repositories
{
    public class OrdenCompraRepository : IOrdenCompraRepository
    {
        private readonly IDbContext _dbContext;

        public OrdenCompraRepository(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task<OrdenCompra> SaveAsync(OrdenCompra ordenCompra)
        {
            ordenCompra.id = await _dbContext.Connection.InsertAsync(ordenCompra);
            return ordenCompra;
        }

        public async Task<OrdenCompra> UpdateAsync(OrdenCompra ordenCompra)
        {
            await _dbContext.Connection.UpdateAsync(ordenCompra);
            return ordenCompra;
        }

        public async Task<List<OrdenCompra>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Orden" +
                               "Compra WHERE IsDeleted = 0";
            var ordenesCompra = await _dbContext.Connection.QueryAsync<OrdenCompra>(sql);
            return ordenesCompra.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ordenCompra = await GetById(id);
            if (ordenCompra == null)
                return false;

            ordenCompra.IsDeleted = true;
            await _dbContext.Connection.UpdateAsync(ordenCompra);
            return true;
        }

        public async Task<OrdenCompra> GetById(int id)
        {
            var ordenCompra = await _dbContext.Connection.GetAsync<OrdenCompra>(id);
            if (ordenCompra == null || ordenCompra.IsDeleted)
                return null;

            return ordenCompra;
        }
    }
}