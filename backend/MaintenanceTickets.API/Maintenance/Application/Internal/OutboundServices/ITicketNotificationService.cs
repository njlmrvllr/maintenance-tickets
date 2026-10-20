using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service that tells the reporter of a ticket about its status changes.
/// </summary>
/// <remarks>
///     The application layer only knows this contract. The infrastructure layer decides the channel
///     (email, SMS, push...). In this project the implementation is simulated and writes to the log.
/// </remarks>
public interface ITicketNotificationService
{
    /// <summary>
    ///     Notifies the reporter that the ticket status changed.
    /// </summary>
    /// <param name="ticket">The ticket with its new status, as stored in the database.</param>
    /// <param name="previousStatus">The status the ticket had before the change.</param>
    /// <param name="changedBy">The operator who made the change.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task NotifyStatusChangedAsync(
        Ticket ticket,
        TicketStatus previousStatus,
        PersonName changedBy,
        CancellationToken cancellationToken = default);
}
