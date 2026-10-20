using MaintenanceTickets.API.Maintenance.Application.Errors;
using MaintenanceTickets.API.Maintenance.Application.Services;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Commands;
using MaintenanceTickets.API.Maintenance.Domain.Repositories;
using MaintenanceTickets.API.Shared.Application.Patterns;

namespace MaintenanceTickets.API.Maintenance.Application.Internal.CommandServices;

/// <summary>
///     Application service that handles ticket commands.
/// </summary>
/// <param name="ticketRepository">Repository for persisting tickets.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class TicketCommandService(
    ITicketRepository ticketRepository,
    ILogger<TicketCommandService> logger)
    : ITicketCommandService
{
    /// <inheritdoc />
    public async Task<Result<Ticket, CreateTicketError>> Handle(CreateTicketCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ticket = new Ticket(command);
            var persistedTicket = await ticketRepository.AddAsync(ticket, cancellationToken);
            return new Result<Ticket, CreateTicketError>.Success(persistedTicket);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Unexpected error creating a ticket for asset {AssetCode}", command.AssetCode);
            return new Result<Ticket, CreateTicketError>.Failure(CreateTicketError.UnexpectedError);
        }
    }
}