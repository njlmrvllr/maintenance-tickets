namespace MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Lifecycle status of a maintenance ticket.
/// </summary>
/// <remarks>
///     The mapping to the database values (PENDING, IN_PROGRESS...) lives in the infrastructure layer,
///     so the domain does not depend on persistence details.
/// </remarks>
public enum TicketStatus
{
    Pending,
    InProgress,
    Resolved,
    Closed,
    Cancelled
}