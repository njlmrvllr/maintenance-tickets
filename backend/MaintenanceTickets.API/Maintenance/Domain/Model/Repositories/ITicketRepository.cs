using MaintenanceTickets.API.Maintenance.Domain.Exceptions;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Entities;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Domain.Model.Repositories;

/// <summary>
///     The ticket repository contract.
/// </summary>
/// <remarks>
///     Implemented in the infrastructure layer with Dapper and stored procedures.
///     It does not extend a generic base repository because persistence goes through
///     stored procedures instead of a change tracker.
/// </remarks>
public interface ITicketRepository
{
    /// <summary>
    ///     Persists a new ticket and its first history record in a single transaction.
    /// </summary>
    /// <returns>The persisted ticket, including its generated id and timestamps.</returns>
    Task<Ticket> AddAsync(Ticket ticket, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds a ticket by id.
    /// </summary>
    /// <returns>The ticket if found, or null otherwise.</returns>
    Task<Ticket?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Lists tickets, newest first, optionally filtered by status.
    /// </summary>
    Task<IEnumerable<Ticket>> ListAsync(TicketStatus? status = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Saves a status change and its history record in a single transaction.
    /// </summary>
    /// <param name="ticket">The ticket with its new status already applied.</param>
    /// <param name="expectedStatus">The status the ticket had before the change.</param>
    /// <param name="comment">The comment for the history record.</param>
    /// <param name="changedBy">The operator making the change.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated ticket as stored in the database.</returns>
    /// <exception cref="TicketConcurrencyException">
    ///     Thrown when the ticket status was changed by another operation in the meantime.
    /// </exception>
    Task<Ticket> UpdateStatusAsync(
        Ticket ticket,
        TicketStatus expectedStatus,
        string? comment,
        PersonName changedBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the history of a ticket in chronological order.
    /// </summary>
    Task<IEnumerable<TicketHistoryEntry>> FindHistoryByTicketIdAsync(
        int ticketId,
        CancellationToken cancellationToken = default);
}