namespace MaintenanceTickets.API.Maintenance.Domain.Model;

/// <summary>
///     Outcome of trying to move a ticket to a new status.
/// </summary>
/// <remarks>
///     The domain returns this enum instead of the application Result pattern,
///     so the domain layer does not depend on the application layer.
/// </remarks>
public enum TicketTransitionOutcome
{
    /// <summary>The transition was valid and the ticket status changed.</summary>
    Accepted,

    /// <summary>The state machine does not allow moving from the current status to the target one.</summary>
    InvalidTransition,

    /// <summary>A ticket cannot be resolved without a diagnosis.</summary>
    DiagnosisRequired,

    /// <summary>A ticket cannot be cancelled without a reason.</summary>
    ReasonRequired
}