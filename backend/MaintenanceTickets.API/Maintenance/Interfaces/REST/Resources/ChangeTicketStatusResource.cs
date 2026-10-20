using System.ComponentModel.DataAnnotations;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

/// <summary>
///     Request payload to move a ticket to a new status.
/// </summary>
[SwaggerSchema(Description = "Request payload to move a ticket to a new status")]
public record ChangeTicketStatusResource(
    [Required]
    [SwaggerParameter(Description = "Target status: InProgress, Resolved, Closed or Cancelled")]
    TicketStatus? TargetStatus,

    [MaxLength(500)]
    [SwaggerParameter(Description = "Diagnosis (required to resolve), reason (required to cancel) or an optional note")]
    string? Comment,

    [Required]
    [SwaggerParameter(Description = "Name of the operator making the change")]
    string ChangedBy);