using MaintenanceTickets.API.Maintenance.Application.Errors;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of changing a ticket status.
/// </summary>
/// <remarks>
///     Maps each application error to the HTTP status shown in the sequence diagram:
///     404 not found, 409 invalid transition or concurrent change, 400 missing diagnosis/reason, 500 unexpected.
/// </remarks>
public static class ActionResultFromChangeTicketStatusResultAssembler
{
    /// <summary>
    ///     Maps the application result to 200 OK or to a ProblemDetails error.
    /// </summary>
    /// <param name="result">The result produced by the command service.</param>
    /// <param name="controller">The controller used to produce HTTP results.</param>
    public static ActionResult ToActionResultFromChangeTicketStatusResult(
        Result<Ticket, ChangeTicketStatusError> result,
        ControllerBase controller) =>
        result switch
        {
            Result<Ticket, ChangeTicketStatusError>.Success success =>
                controller.Ok(TicketResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Ticket, ChangeTicketStatusError>.Failure failure => failure.Error switch
            {
                ChangeTicketStatusError.TicketNotFound => controller.Problem(
                    title: "Ticket not found",
                    detail: "The requested ticket does not exist.",
                    statusCode: StatusCodes.Status404NotFound),

                ChangeTicketStatusError.InvalidTransition => controller.Problem(
                    title: "Transition not allowed",
                    detail: "The ticket cannot move to the requested status from its current status.",
                    statusCode: StatusCodes.Status409Conflict),

                ChangeTicketStatusError.DiagnosisRequired => controller.Problem(
                    title: "Diagnosis required",
                    detail: "A diagnosis is required to resolve a ticket. Send it in the comment field.",
                    statusCode: StatusCodes.Status400BadRequest),

                ChangeTicketStatusError.ReasonRequired => controller.Problem(
                    title: "Reason required",
                    detail: "A reason is required to cancel a ticket. Send it in the comment field.",
                    statusCode: StatusCodes.Status400BadRequest),

                ChangeTicketStatusError.ConcurrentModification => controller.Problem(
                    title: "Ticket was modified",
                    detail: "Another operator changed this ticket in the meantime. Reload it and try again.",
                    statusCode: StatusCodes.Status409Conflict),

                _ => controller.Problem(
                    title: "Unexpected server error",
                    detail: "An unexpected error occurred while changing the ticket status.",
                    statusCode: StatusCodes.Status500InternalServerError)
            },

            _ => controller.Problem(
                title: "Unexpected server error",
                detail: "An unexpected error occurred while processing your request.",
                statusCode: StatusCodes.Status500InternalServerError)
        };
}