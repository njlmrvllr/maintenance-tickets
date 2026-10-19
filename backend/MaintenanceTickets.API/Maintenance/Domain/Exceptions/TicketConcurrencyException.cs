namespace MaintenanceTickets.API.Maintenance.Domain.Exceptions;

/// <summary>
///     Thrown when a ticket status was changed by another operation between reading and updating it.
/// </summary>
/// <remarks>
///     The infrastructure layer raises it when the stored procedure signals error 50409.
///     The application layer translates it into a 409 Conflict.
/// </remarks>
public class TicketConcurrencyException(int ticketId)
    : Exception($"Ticket {ticketId} was changed by another operation. Reload it and try again.")
{
    /// <summary>Gets the ticket that had the conflict.</summary>
    public int TicketId { get; } = ticketId;
}