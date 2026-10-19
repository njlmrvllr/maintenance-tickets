using System.Data.Common;
using MySqlConnector;

namespace MaintenanceTickets.API.Shared.Infrastructure.Persistence.Dapper;

/// <summary>
///     MySQL implementation of <see cref="IDbConnectionFactory"/>.
/// </summary>
/// <remarks>
///     The connection string is read from configuration (user secrets in development,
///     environment variables in production). It is never hardcoded in the source code.
///     Environment variable placeholders such as %MYSQLHOST% are expanded.
/// </remarks>
public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    ///     Initializes the factory with the DefaultConnection connection string.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <exception cref="InvalidOperationException">Thrown when the connection string is not configured.</exception>
    public MySqlConnectionFactory(IConfiguration configuration)
    {
        var connectionStringTemplate = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionStringTemplate))
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured. Set it with user secrets or environment variables.");

        _connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    }

    /// <inheritdoc />
    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}