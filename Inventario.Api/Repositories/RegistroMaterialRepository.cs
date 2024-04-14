using Dapper;
using Inventario.Core.Entities;
using Inventario.Api.DataAccess.Interfaces;
using Inventario.Api.Repositories.Interfecies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace Inventario.Api.Repositories
{
    public class RegistroMaterialRepository : IRegistroMaterialRepository
    {
        private readonly IDbContext _dbContext;

        public RegistroMaterialRepository(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task<RegistroMaterial> SaveAsync(RegistroMaterial registroMaterial)
        {
            registroMaterial.id = await _dbContext.Connection.InsertAsync(registroMaterial);
            return registroMaterial;
        }

        public async Task<RegistroMaterial> UpdateAsync(RegistroMaterial registroMaterial)
        {
            await _dbContext.Connection.UpdateAsync(registroMaterial);
            return registroMaterial;
        }

        public async Task<List<RegistroMaterial>> GetAllAsync()
        {
            const string sql = "SELECT * FROM RegistroMaterial WHERE IsDeleted = 0";
            var registros = await _dbContext.Connection.QueryAsync<RegistroMaterial>(sql);
            return registros.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var registroMaterial = await GetById(id);
            if (registroMaterial == null)
                return false;

            registroMaterial.IsDeleted = true;
            return await _dbContext.Connection.UpdateAsync(registroMaterial);
        }

        public async Task<RegistroMaterial> GetById(int id)
        {
            var registroMaterial = await _dbContext.Connection.GetAsync<RegistroMaterial>(id);
            if (registroMaterial == null || registroMaterial.IsDeleted)
                return null;
            
            return registroMaterial;
        }
    }
}