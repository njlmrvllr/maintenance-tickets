using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Infrastructure.Persistence.Dapper;

/// <summary>
///     Translates <see cref="TicketStatus"/> values to and from the values stored in the database.
/// </summary>
/// <remarks>
///     Keeps the database representation (PENDING, IN_PROGRESS...) out of the domain layer.
/// </remarks>
public static class TicketStatusMapper
{
    /// <summary>
    ///     Converts a domain status to its database value.
    /// </summary>
    public static string ToDatabaseValue(TicketStatus status) => status switch
    {
        TicketStatus.Pending => "PENDING",
        TicketStatus.InProgress => "IN_PROGRESS",
        TicketStatus.Resolved => "RESOLVED",
        TicketStatus.Closed => "CLOSED",
        TicketStatus.Cancelled => "CANCELLED",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown ticket status.")
    };

    /// <summary>
    ///     Converts a database value to its domain status.
    /// </summary>
    public static TicketStatus FromDatabaseValue(string value) => value switch
    {
        "PENDING" => TicketStatus.Pending,
        "IN_PROGRESS" => TicketStatus.InProgress,
        "RESOLVED" => TicketStatus.Resolved,
        "CLOSED" => TicketStatus.Closed,
        "CANCELLED" => TicketStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown ticket status in the database.")
    };
}