using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to report a new maintenance incident.
/// </summary>
/// <param name="Title">Short title of the incident.</param>
/// <param name="AssetCode">Code of the affected equipment or facility.</param>
/// <param name="Description">Detailed description of the incident.</param>
/// <param name="ReportedBy">Name of the person reporting the incident.</param>
public record CreateTicketCommand(
    TicketTitle Title,
    AssetCode AssetCode,
    TicketDescription Description,
    PersonName ReportedBy);