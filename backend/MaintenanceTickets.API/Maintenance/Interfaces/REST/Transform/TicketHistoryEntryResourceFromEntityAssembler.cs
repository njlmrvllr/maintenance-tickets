using MaintenanceTickets.API.Maintenance.Domain.Model.Entities;
using MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Assembles a TicketHistoryEntryResource from a TicketHistoryEntry.
/// </summary>
public static class TicketHistoryEntryResourceFromEntityAssembler
{
    /// <summary>
    ///     Builds the resource from the history entry.
    /// </summary>
    public static TicketHistoryEntryResource ToResourceFromEntity(TicketHistoryEntry entity) =>
        new(
            entity.Id,
            entity.FromStatus,
            entity.ToStatus,
            entity.Comment,
            entity.ChangedBy,
            entity.ChangedAt);
}