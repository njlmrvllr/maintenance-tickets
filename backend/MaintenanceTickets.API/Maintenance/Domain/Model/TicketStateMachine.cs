using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Domain.Model;

/// <summary>
///     Defines which status transitions are allowed for a ticket.
/// </summary>
/// <remarks>
///     Pending -> InProgress | Cancelled
///     InProgress -> Resolved | Cancelled
///     Resolved -> Closed | Cancelled
///     Closed and Cancelled are final states.
/// </remarks>
public static class TicketStateMachine
{
    private static readonly IReadOnlyDictionary<TicketStatus, TicketStatus[]> AllowedTransitions =
        new Dictionary<TicketStatus, TicketStatus[]>
        {
            [TicketStatus.Pending] = [TicketStatus.InProgress, TicketStatus.Cancelled],
            [TicketStatus.InProgress] = [TicketStatus.Resolved, TicketStatus.Cancelled],
            [TicketStatus.Resolved] = [TicketStatus.Closed, TicketStatus.Cancelled],
            [TicketStatus.Closed] = [],
            [TicketStatus.Cancelled] = []
        };

    /// <summary>
    ///     Checks whether a ticket can move from one status to another.
    /// </summary>
    /// <param name="from">The current status.</param>
    /// <param name="to">The target status.</param>
    /// <returns>True if the transition is allowed; otherwise false.</returns>
    public static bool CanTransition(TicketStatus from, TicketStatus to) =>
        AllowedTransitions.TryGetValue(from, out var targets) && targets.Contains(to);

    /// <summary>
    ///     Gets the statuses a ticket can move to from its current status.
    /// </summary>
    /// <remarks>
    ///     Useful for the frontend, to show only the valid actions for each ticket.
    /// </remarks>
    public static IReadOnlyCollection<TicketStatus> GetAllowedTargets(TicketStatus from) =>
        AllowedTransitions.TryGetValue(from, out var targets) ? targets : Array.Empty<TicketStatus>();
}