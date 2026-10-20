using MaintenanceTickets.API.Maintenance.Domain.Model.Commands;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Assembles a CreateTicketCommand from a CreateTicketResource.
/// </summary>
public static class CreateTicketCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds the command. The value objects validate the input and throw ArgumentException if it is invalid.
    /// </summary>
    public static CreateTicketCommand ToCommandFromResource(CreateTicketResource resource) =>
        new(
            new TicketTitle(resource.Title),
            new AssetCode(resource.AssetCode),
            new TicketDescription(resource.Description),
            new PersonName(resource.ReportedBy));
}