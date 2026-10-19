using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Policies;
using MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Assembles a TicketResource from a Ticket aggregate.
/// </summary>
public static class TicketResourceFromEntityAssembler
{
    /// <summary>
    ///     Builds the resource, including the statuses the ticket can move to.
    /// </summary>
    public static TicketResource ToResourceFromEntity(Ticket entity) =>
        new(
            entity.Id,
            entity.Title.Value,
            entity.AssetCode.Value,
            entity.Description.Value,
            entity.Status,
            entity.Diagnosis,
            entity.ReportedBy.Value,
            TicketStateMachine.GetAllowedTargets(entity.Status),
            entity.CreatedAt,
            entity.UpdatedAt);
}