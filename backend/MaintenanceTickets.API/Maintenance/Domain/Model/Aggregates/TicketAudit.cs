namespace MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;

/// <summary>
///     Audit extension for the Ticket aggregate.
/// </summary>
/// <remarks>
///     The timestamps are set by the database (DEFAULT CURRENT_TIMESTAMP),
///     so they are only assigned when a ticket is restored from persistence.
/// </remarks>
public partial class Ticket
{
    /// <summary>Gets when the ticket was created. Maps to column: created_at.</summary>
    public DateTimeOffset? CreatedAt { get; private set; }

    /// <summary>Gets when the ticket was last updated. Maps to column: updated_at.</summary>
    public DateTimeOffset? UpdatedAt { get; private set; }
}