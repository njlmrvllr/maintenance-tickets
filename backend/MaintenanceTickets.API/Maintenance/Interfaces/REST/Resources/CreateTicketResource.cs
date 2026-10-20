using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

/// <summary>
///     Request payload to report a maintenance incident.
/// </summary>
[SwaggerSchema(Description = "Request payload to report a maintenance incident")]
public record CreateTicketResource(
    [Required]
    [SwaggerParameter(Description = "Short title of the incident (max 120 characters)")]
    string Title,

    [Required]
    [SwaggerParameter(Description = "Code of the affected equipment or facility, e.g. ATM-0451")]
    string AssetCode,

    [Required]
    [SwaggerParameter(Description = "Detailed description of the incident (max 1000 characters)")]
    string Description,

    [Required]
    [SwaggerParameter(Description = "Name of the person reporting the incident")]
    string ReportedBy);