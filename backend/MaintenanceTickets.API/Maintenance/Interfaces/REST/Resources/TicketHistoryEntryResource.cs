using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

/// <summary>
///     A history event of a ticket as returned by the API.
/// </summary>
[SwaggerSchema(Description = "A status change in the history of a ticket")]
public record TicketHistoryEntryResource(
    int Id,
    TicketStatus? FromStatus,
    TicketStatus ToStatus,
    string? Comment,
    string ChangedBy,
    DateTimeOffset ChangedAt);