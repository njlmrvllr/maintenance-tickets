namespace MaintenanceTickets.API.Maintenance.Application.Errors;

/// <summary>
///     Errors that can occur when creating a ticket.
/// </summary>
/// <remarks>
///     Invalid input is rejected earlier by the value objects (400 Bad Request),
///     so the only remaining failure is an unexpected one.
/// </remarks>
public enum CreateTicketError
{
    /// <summary>An unexpected error occurred while saving the ticket.</summary>
    UnexpectedError
}