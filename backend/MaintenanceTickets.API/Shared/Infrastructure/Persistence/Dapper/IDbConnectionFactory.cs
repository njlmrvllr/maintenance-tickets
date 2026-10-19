using System.Data.Common;

namespace MaintenanceTickets.API.Shared.Infrastructure.Persistence.Dapper;

/// <summary>
///     Creates open database connections for the Dapper repositories.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    ///     Creates and opens a new database connection. The caller must dispose it.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>An open database connection.</returns>
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}