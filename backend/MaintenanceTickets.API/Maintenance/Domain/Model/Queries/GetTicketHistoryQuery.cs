namespace MaintenanceTickets.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to get the history of a ticket.
/// </summary>
/// <param name="TicketId">The ticket id.</param>
public record GetTicketHistoryQuery(int TicketId);