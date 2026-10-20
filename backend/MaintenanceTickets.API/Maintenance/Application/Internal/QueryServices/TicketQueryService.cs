using MaintenanceTickets.API.Maintenance.Application.Services;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Entities;
using MaintenanceTickets.API.Maintenance.Domain.Model.Queries;
using MaintenanceTickets.API.Maintenance.Domain.Repositories;

namespace MaintenanceTickets.API.Maintenance.Application.Internal.QueryServices;

/// <summary>
///     Application service that handles ticket queries.
/// </summary>
/// <param name="ticketRepository">Repository for reading tickets.</param>
public class TicketQueryService(ITicketRepository ticketRepository) : ITicketQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Ticket>> Handle(GetAllTicketsQuery query,
        CancellationToken cancellationToken = default) =>
        await ticketRepository.ListAsync(query.Status, cancellationToken);

    /// <inheritdoc />
    public async Task<Ticket?> Handle(GetTicketByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await ticketRepository.FindByIdAsync(query.TicketId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<TicketHistoryEntry>?> Handle(GetTicketHistoryQuery query,
        CancellationToken cancellationToken = default)
    {
        var ticket = await ticketRepository.FindByIdAsync(query.TicketId, cancellationToken);
        if (ticket is null) return null;

        return await ticketRepository.FindHistoryByTicketIdAsync(query.TicketId, cancellationToken);
    }
}