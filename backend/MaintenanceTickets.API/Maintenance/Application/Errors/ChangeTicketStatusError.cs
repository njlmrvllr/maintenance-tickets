namespace MaintenanceTickets.API.Maintenance.Application.Errors;

/// <summary>
///     Errors that can occur when changing the status of a ticket.
/// </summary>
public enum ChangeTicketStatusError
{
    /// <summary>
    /// The ticket does not exist.</summary>
    TicketNotFound,

    /// <summary>
    /// The state machine does not allow moving to the requested status.
    /// </summary>
    InvalidTransition,

    /// <summary>
    /// A ticket cannot be resolved without a diagnosis.
    /// </summary>
    DiagnosisRequired,

    /// <summary>
    /// A ticket cannot be cancelled without a reason.
    /// </summary>
    ReasonRequired,

    /// <summary>
    /// Another operation changed the ticket status in the meantime.
    /// </summary>
    ConcurrentModification,

    /// <summary>
    /// An unexpected error occurred while saving the change.
    /// </summary>
    UnexpectedError
}