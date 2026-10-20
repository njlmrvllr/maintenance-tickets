using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

namespace MaintenanceTickets.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to list tickets, optionally filtered by status.
/// </summary>
/// <param name="Status">The status to filter by, or null to get every ticket.</param>
public record GetAllTicketsQuery(TicketStatus? Status = null);