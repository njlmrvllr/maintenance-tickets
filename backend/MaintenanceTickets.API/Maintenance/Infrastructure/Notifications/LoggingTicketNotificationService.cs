using MaintenanceTickets.API.Maintenance.Application.Internal.OutboundServices;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Infrastructure.Notifications;

/// <summary>
///     Simulated <see cref="ITicketNotificationService"/>: instead of sending a real message,
///     it writes the notification to the application log.
/// </summary>
/// <remarks>
///     A real channel (email, SMS) can replace this class without changing the application layer.
/// </remarks>
/// <param name="logger">Logger used as the simulated delivery channel.</param>
public class LoggingTicketNotificationService(ILogger<LoggingTicketNotificationService> logger)
    : ITicketNotificationService
{
    /// <inheritdoc />
    public Task NotifyStatusChangedAsync(
        Ticket ticket,
        TicketStatus previousStatus,
        PersonName changedBy,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "[Simulated notification] To: {Recipient} | Ticket #{TicketId} \"{Title}\" moved from {FromStatus} to {ToStatus} by {ChangedBy}",
            ticket.ReportedBy.Value, ticket.Id, ticket.Title.Value, previousStatus, ticket.Status, changedBy.Value);

        return Task.CompletedTask;
    }
}
