using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Domain.Model.Entities;

/// <summary>
///     A single event in the history of a ticket.
/// </summary>
/// <param name="Id">The server-generated identifier.</param>
/// <param name="TicketId">The ticket this event belongs to.</param>
/// <param name="FromStatus">The previous status, or null when the ticket was created.</param>
/// <param name="ToStatus">The new status.</param>
/// <param name="Comment">Diagnosis, reason or note attached to the change.</param>
/// <param name="ChangedBy">Name of the person who made the change.</param>
/// <param name="ChangedAt">When the change happened.</param>
public record TicketHistoryEntry(
    int Id,
    int TicketId,
    TicketStatus? FromStatus,
    TicketStatus ToStatus,
    string? Comment,
    string ChangedBy,
    DateTimeOffset ChangedAt);