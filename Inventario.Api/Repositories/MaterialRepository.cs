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
    public class MaterialRepository : IMaterialRepository
    {
        private readonly IDbContext _dbContext;

        public MaterialRepository(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Material> SaveAsync(Material material)
        {
            material.id = await _dbContext.Connection.InsertAsync(material);
            return material;
        }

        public async Task<Material> UpdateAsync(Material material)
        {
            await _dbContext.Connection.UpdateAsync(material);
            return material;
        }

        public async Task<List<Material>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Material WHERE IsDeleted = 0";
            var materials = await _dbContext.Connection.QueryAsync<Material>(sql);
            return materials.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var material = await GetById(id);
            if (material == null)
                return false;

            material.IsDeleted = true;
            await _dbContext.Connection.UpdateAsync(material);
            return true;
        }

        public async Task<Material> GetById(int id)
        {
            var material = await _dbContext.Connection.GetAsync<Material>(id);
            if (material == null || material.IsDeleted)
                return null;

            return material;
        }
    }
}