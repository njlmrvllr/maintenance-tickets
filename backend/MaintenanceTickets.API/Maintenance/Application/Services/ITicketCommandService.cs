using MaintenanceTickets.API.Maintenance.Application.Errors;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Commands;
using MaintenanceTickets.API.Shared.Application.Patterns;

namespace MaintenanceTickets.API.Maintenance.Application.Services;

/// <summary>
///     Command service for ticket operations that change state.
/// </summary>
public interface ITicketCommandService
{
    /// <summary>
    ///     Handles the creation of a new ticket.
    /// </summary>
    /// <param name="command">The CreateTicketCommand command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created ticket on success, or the error reason on failure.</returns>
    Task<Result<Ticket, CreateTicketError>> Handle(CreateTicketCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles a status change, applying the state machine rules of the Ticket aggregate.
    /// </summary>
    /// <param name="command">The ChangeTicketStatusCommand command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated ticket on success, or the error reason on failure.</returns>
    Task<Result<Ticket, ChangeTicketStatusError>> Handle(ChangeTicketStatusCommand command,
        CancellationToken cancellationToken = default);
}