namespace MaintenanceTickets.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to get a ticket by its id.
/// </summary>
/// <param name="TicketId">The ticket id.</param>
public record GetTicketByIdQuery(int TicketId);