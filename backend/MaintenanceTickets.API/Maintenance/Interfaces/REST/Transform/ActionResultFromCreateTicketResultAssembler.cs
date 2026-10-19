using MaintenanceTickets.API.Maintenance.Application.Errors;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of creating a ticket.
/// </summary>
public static class ActionResultFromCreateTicketResultAssembler
{
    /// <summary>
    ///     Maps the application result to 201 Created or to a ProblemDetails error.
    /// </summary>
    /// <param name="result">The result produced by the command service.</param>
    /// <param name="controller">The controller used to produce HTTP results.</param>
    /// <param name="getTicketByIdActionName">The action used to build the Location header.</param>
    public static ActionResult ToActionResultFromCreateTicketResult(
        Result<Ticket, CreateTicketError> result,
        ControllerBase controller,
        string getTicketByIdActionName) =>
        result switch
        {
            Result<Ticket, CreateTicketError>.Success success =>
                controller.CreatedAtAction(
                    getTicketByIdActionName,
                    new { id = success.Value.Id },
                    TicketResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            _ => controller.Problem(
                title: "Unexpected server error",
                detail: "An unexpected error occurred while creating the ticket.",
                statusCode: StatusCodes.Status500InternalServerError)
        };
}