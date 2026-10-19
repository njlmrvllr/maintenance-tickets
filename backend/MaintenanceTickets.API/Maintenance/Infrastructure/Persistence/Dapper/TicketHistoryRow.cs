using MaintenanceTickets.API.Maintenance.Domain.Model.Entities;

namespace MaintenanceTickets.API.Maintenance.Infrastructure.Persistence.Dapper;

/// <summary>
///     Row returned by sp_get_ticket_history.
/// </summary>
public sealed class TicketHistoryRow
{
    public int Id { get; init; }
    public int TicketId { get; init; }
    public string? FromStatus { get; init; }
    public string ToStatus { get; init; } = string.Empty;
    public string? Comment { get; init; }
    public string ChangedBy { get; init; } = string.Empty;
    public DateTime ChangedAt { get; init; }

    /// <summary>
    ///     Converts this row into a domain history entry.
    /// </summary>
    public TicketHistoryEntry ToEntry() =>
        new(
            Id,
            TicketId,
            FromStatus is null ? null : TicketStatusMapper.FromDatabaseValue(FromStatus),
            TicketStatusMapper.FromDatabaseValue(ToStatus),
            Comment,
            ChangedBy,
            new DateTimeOffset(DateTime.SpecifyKind(ChangedAt, DateTimeKind.Utc)));
}