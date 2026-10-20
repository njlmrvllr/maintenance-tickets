using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to move a ticket to a new status.
/// </summary>
/// <param name="TicketId">The ticket to update.</param>
/// <param name="TargetStatus">The status to move the ticket to.</param>
/// <param name="Comment">Diagnosis when resolving, reason when cancelling, or an optional note otherwise.</param>
/// <param name="ChangedBy">Name of the operator making the change.</param>
public record ChangeTicketStatusCommand(
    int TicketId,
    TicketStatus TargetStatus,
    string? Comment,
    PersonName ChangedBy);