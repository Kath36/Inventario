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
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly IDbContext _dbContext;

        public ProveedorRepository(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Proveedor> SaveAsync(Proveedor proveedor)
        {
            proveedor.id = await _dbContext.Connection.InsertAsync(proveedor);
            return proveedor;
        }

        public async Task<Proveedor> UpdateAsync(Proveedor proveedor)
        {
            await _dbContext.Connection.UpdateAsync(proveedor);
            return proveedor;
        }

        public async Task<List<Proveedor>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Proveedor WHERE IsDeleted = 0";
            var proveedores = await _dbContext.Connection.QueryAsync<Proveedor>(sql);
            return proveedores.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var proveedor = await GetById(id);
            if (proveedor == null)
                return false;

            proveedor.IsDeleted = true;
            return await _dbContext.Connection.UpdateAsync(proveedor);
        }

        public async Task<Proveedor> GetById(int id)
        {
            var proveedor = await _dbContext.Connection.GetAsync<Proveedor>(id);
            if (proveedor == null || proveedor.IsDeleted)
                return null;
            
            return proveedor;
        }
    }
}