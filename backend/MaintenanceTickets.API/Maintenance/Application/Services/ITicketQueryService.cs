using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Entities;
using MaintenanceTickets.API.Maintenance.Domain.Model.Queries;

namespace MaintenanceTickets.API.Maintenance.Application.Services;

/// <summary>
///     Query service for reading tickets.
/// </summary>
public interface ITicketQueryService
{
    /// <summary>
    ///     Lists tickets, newest first, optionally filtered by status.
    /// </summary>
    Task<IEnumerable<Ticket>> Handle(GetAllTicketsQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets a ticket by id.
    /// </summary>
    /// <returns>The ticket if found, or null otherwise.</returns>
    Task<Ticket?> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the history of a ticket in chronological order.
    /// </summary>
    /// <returns>The history entries, or null when the ticket does not exist.</returns>
    Task<IEnumerable<TicketHistoryEntry>?> Handle(GetTicketHistoryQuery query,
        CancellationToken cancellationToken = default);
}