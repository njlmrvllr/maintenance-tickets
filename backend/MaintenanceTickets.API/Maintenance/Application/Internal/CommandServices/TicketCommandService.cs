using MaintenanceTickets.API.Maintenance.Application.Errors;
using MaintenanceTickets.API.Maintenance.Application.Internal.OutboundServices;
using MaintenanceTickets.API.Maintenance.Application.Services;
using MaintenanceTickets.API.Maintenance.Domain.Exceptions;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Commands;
using MaintenanceTickets.API.Maintenance.Domain.Model.Policies;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using MaintenanceTickets.API.Maintenance.Domain.Repositories;
using MaintenanceTickets.API.Shared.Application.Patterns;

namespace MaintenanceTickets.API.Maintenance.Application.Internal.CommandServices;

/// <summary>
///     Application service that handles ticket commands.
/// </summary>
/// <param name="ticketRepository">Repository for persisting tickets.</param>
/// <param name="notificationService">Outbound service that notifies the reporter of status changes.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class TicketCommandService(
    ITicketRepository ticketRepository,
    ITicketNotificationService notificationService,
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

    /// <inheritdoc />
    /// <remarks>
    ///     Follows the "Change Ticket Status" sequence diagram:
    ///     find the ticket, let the aggregate validate the transition, then save the change
    ///     and its history record through the repository (stored procedure, single transaction).
    /// </remarks>
    public async Task<Result<Ticket, ChangeTicketStatusError>> Handle(ChangeTicketStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ticket = await ticketRepository.FindByIdAsync(command.TicketId, cancellationToken);
            if (ticket is null)
                return new Result<Ticket, ChangeTicketStatusError>.Failure(ChangeTicketStatusError.TicketNotFound);

            var previousStatus = ticket.Status;
            var outcome = ticket.ChangeStatus(command.TargetStatus, command.Comment);

            if (outcome != TicketTransitionOutcome.Accepted)
            {
                logger.LogWarning(
                    "Status change rejected for ticket {TicketId} from {FromStatus} to {ToStatus}: {Outcome}",
                    command.TicketId, previousStatus, command.TargetStatus, outcome);
                return new Result<Ticket, ChangeTicketStatusError>.Failure(ToError(outcome));
            }

            var comment = string.IsNullOrWhiteSpace(command.Comment) ? null : command.Comment.Trim();
            var updatedTicket = await ticketRepository.UpdateStatusAsync(
                ticket, previousStatus, comment, command.ChangedBy, cancellationToken);

            await NotifyReporterAsync(updatedTicket, previousStatus, command.ChangedBy, cancellationToken);

            return new Result<Ticket, ChangeTicketStatusError>.Success(updatedTicket);
        }
        catch (TicketConcurrencyException ex)
        {
            logger.LogWarning(ex, "Concurrent status change detected for ticket {TicketId}", command.TicketId);
            return new Result<Ticket, ChangeTicketStatusError>.Failure(ChangeTicketStatusError.ConcurrentModification);
        }
        catch (Exception ex) when (ex is not OperationCanceledException and not ArgumentException)
        {
            logger.LogError(ex, "Unexpected error changing the status of ticket {TicketId}", command.TicketId);
            return new Result<Ticket, ChangeTicketStatusError>.Failure(ChangeTicketStatusError.UnexpectedError);
        }
    }

    /// <summary>
    ///     Notifies the reporter after the status change is saved.
    /// </summary>
    /// <remarks>
    ///     The change is already committed, so a notification failure is logged and never
    ///     turns a successful status change into an error.
    /// </remarks>
    private async Task NotifyReporterAsync(Ticket ticket, TicketStatus previousStatus, PersonName changedBy,
        CancellationToken cancellationToken)
    {
        try
        {
            await notificationService.NotifyStatusChangedAsync(ticket, previousStatus, changedBy, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "The status change notification for ticket {TicketId} could not be sent", ticket.Id);
        }
    }

    /// <summary>
    ///     Translates the domain outcome into an application error.
    /// </summary>
    private static ChangeTicketStatusError ToError(TicketTransitionOutcome outcome) => outcome switch
    {
        TicketTransitionOutcome.InvalidTransition => ChangeTicketStatusError.InvalidTransition,
        TicketTransitionOutcome.DiagnosisRequired => ChangeTicketStatusError.DiagnosisRequired,
        TicketTransitionOutcome.ReasonRequired => ChangeTicketStatusError.ReasonRequired,
        _ => ChangeTicketStatusError.UnexpectedError
    };
}