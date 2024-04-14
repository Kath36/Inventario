using System.Data.Common;
using MySqlConnector;
using Inventario.Api.DataAccess.Interfaces;

namespace Inventario.Api.DataAccess;

public class DbContext : IDbContext
{
    private readonly IConfiguration _config;

    public DbContext(IConfiguration config)
    {
        _config = config;
    }
    private MySqlConnection _connection;

    public DbConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                //Se llama la cadena de conexion
                _connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
            return _connection;
        }
    }
}