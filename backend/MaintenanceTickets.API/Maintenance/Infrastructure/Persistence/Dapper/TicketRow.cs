using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Infrastructure.Persistence.Dapper;

/// <summary>
///     Row returned by the ticket stored procedures. Dapper fills it from the tickets columns.
/// </summary>
public sealed class TicketRow
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string AssetCode { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? Diagnosis { get; init; }
    public string ReportedBy { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    ///     Rebuilds the domain aggregate from this row.
    /// </summary>
    public Ticket ToTicket() =>
        Ticket.Restore(
            Id,
            new TicketTitle(Title),
            new AssetCode(AssetCode),
            new TicketDescription(Description),
            TicketStatusMapper.FromDatabaseValue(Status),
            Diagnosis,
            new PersonName(ReportedBy),
            AsUtc(CreatedAt),
            AsUtc(UpdatedAt));

    // MySQL stores DATETIME without a time zone; the container runs in UTC.
    private static DateTimeOffset AsUtc(DateTime value) => new(DateTime.SpecifyKind(value, DateTimeKind.Utc));
}