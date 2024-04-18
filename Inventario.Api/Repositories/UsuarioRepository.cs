using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.VisualBasic;
using Inventario.Core.Entities;
using Inventario.Api.DataAccess.Interfaces;
using Inventario.Api.Repositories.Interfaces;
using Inventario.Api.Repositories.Interfecies;

namespace Inventario.Api.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbContext _dbContext;

        public UsuarioRepository(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            usuario.id = await _dbContext.Connection.InsertAsync(usuario);
            return usuario;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario == null)
                return false;

            usuario.IsDeleted = true;
            return await _dbContext.Connection.UpdateAsync(usuario);
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Usuario WHERE IsDeleted = 0";
            var usuarios = await _dbContext.Connection.QueryAsync<Usuario>(sql);
            return usuarios.ToList();
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _dbContext.Connection.GetAsync<Usuario>(id);
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            return await _dbContext.Connection.UpdateAsync(usuario);
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM Usuario WHERE Email = @Email AND IsDeleted = 0";
            return await _dbContext.Connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public async Task<bool> UsuarioExistsByIdAsync(int id)
        {
            var usuario = await GetByIdAsync(id);
            return usuario != null;
        }

        public async Task<bool> UsuarioExistsByEmailAsync(string email)
        {
            var usuario = await GetByEmailAsync(email);
            return usuario != null;
        }
    }
}
