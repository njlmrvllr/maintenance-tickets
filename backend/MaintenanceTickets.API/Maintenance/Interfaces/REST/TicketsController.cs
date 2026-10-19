using System.Net.Mime;
using MaintenanceTickets.API.Maintenance.Application.Services;
using MaintenanceTickets.API.Maintenance.Domain.Model.Queries;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using MaintenanceTickets.API.Maintenance.Interfaces.REST.Resources;
using MaintenanceTickets.API.Maintenance.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaintenanceTickets.API.Maintenance.Interfaces.REST;

/// <summary>
///     Tickets controller.
/// </summary>
/// <param name="ticketCommandService">The ticket command service.</param>
/// <param name="ticketQueryService">The ticket query service.</param>
/// <param name="logger">Logger for diagnostics and error reporting.</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Tickets")]
public class TicketsController(
    ITicketCommandService ticketCommandService,
    ITicketQueryService ticketQueryService,
    ILogger<TicketsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Reports a new maintenance incident.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a ticket",
        Description = "Reports a maintenance incident. Every new ticket starts in Pending status.",
        OperationId = "CreateTicket")]
    [SwaggerResponse(201, "The ticket was created", typeof(TicketResource))]
    [SwaggerResponse(400, "The request payload is invalid", typeof(ProblemDetails))]
    [SwaggerResponse(500, "Unexpected server error", typeof(ProblemDetails))]
    public async Task<ActionResult> CreateTicket([FromBody] CreateTicketResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createTicketCommand = CreateTicketCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await ticketCommandService.Handle(createTicketCommand, cancellationToken);
            return ActionResultFromCreateTicketResultAssembler.ToActionResultFromCreateTicketResult(
                result, this, nameof(GetTicketById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating a ticket");
            return Problem(title: "Invalid ticket", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    /// <summary>
    ///     Lists tickets, optionally filtered by status.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets all tickets",
        Description = "Returns tickets, newest first. Use the status parameter to filter, e.g. ?status=Pending.",
        OperationId = "GetAllTickets")]
    [SwaggerResponse(200, "List of tickets (may be empty)", typeof(IEnumerable<TicketResource>))]
    public async Task<ActionResult> GetAllTickets([FromQuery] TicketStatus? status,
        CancellationToken cancellationToken)
    {
        var tickets = await ticketQueryService.Handle(new GetAllTicketsQuery(status), cancellationToken);
        var resources = tickets.Select(TicketResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a ticket by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Gets a ticket by id",
        Description = "Returns a single ticket with the statuses it can move to.",
        OperationId = "GetTicketById")]
    [SwaggerResponse(200, "The ticket was found", typeof(TicketResource))]
    [SwaggerResponse(404, "The ticket was not found")]
    public async Task<ActionResult> GetTicketById(int id, CancellationToken cancellationToken)
    {
        var ticket = await ticketQueryService.Handle(new GetTicketByIdQuery(id), cancellationToken);
        if (ticket is null) return NotFound();

        return Ok(TicketResourceFromEntityAssembler.ToResourceFromEntity(ticket));
    }
}