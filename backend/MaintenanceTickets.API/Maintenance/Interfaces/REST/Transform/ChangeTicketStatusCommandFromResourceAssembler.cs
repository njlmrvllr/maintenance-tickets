using MaintenanceTickets.API.Maintenance.Domain.Model.Commands;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Assembles a ChangeTicketStatusCommand from the route id and a ChangeTicketStatusResource.
/// </summary>
public static class ChangeTicketStatusCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds the command. PersonName validates the operator name and throws ArgumentException if it is invalid.
    /// </summary>
    /// <param name="ticketId">The ticket id taken from the route.</param>
    /// <param name="resource">The request payload. TargetStatus is guaranteed by [Required].</param>
    public static ChangeTicketStatusCommand ToCommandFromResource(int ticketId, ChangeTicketStatusResource resource) =>
        new(
            ticketId,
            resource.TargetStatus!.Value,
            resource.Comment,
            new PersonName(resource.ChangedBy));
}