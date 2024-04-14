using System.Data.Common;

namespace Inventario.Api.DataAccess.Interfaces;

public interface IDbContext
{
    DbConnection Connection { get; }
}