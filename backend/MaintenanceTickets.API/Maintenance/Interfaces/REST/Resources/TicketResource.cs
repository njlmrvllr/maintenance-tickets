using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

/// <summary>
///     A ticket as returned by the API.
/// </summary>
[SwaggerSchema(Description = "A maintenance ticket")]
public record TicketResource(
    int Id,
    string Title,
    string AssetCode,
    string Description,
    TicketStatus Status,
    string? Diagnosis,
    string ReportedBy,
    [property: SwaggerSchema(Description = "Statuses this ticket can move to, so the UI only shows valid actions")]
    IReadOnlyCollection<TicketStatus> AllowedTransitions,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt);