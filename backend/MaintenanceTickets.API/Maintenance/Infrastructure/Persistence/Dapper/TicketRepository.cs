using System.Data;
using Dapper;
using MaintenanceTickets.API.Maintenance.Domain.Exceptions;
using MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;
using MaintenanceTickets.API.Maintenance.Domain.Model.Entities;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using MaintenanceTickets.API.Maintenance.Domain.Repositories;
using MaintenanceTickets.API.Shared.Infrastructure.Persistence.Dapper;
using MySqlConnector;

namespace MaintenanceTickets.API.Maintenance.Infrastructure.Persistence.Dapper;

/// <summary>
///     Dapper implementation of <see cref="ITicketRepository"/> backed by MySQL stored procedures.
/// </summary>
/// <param name="connectionFactory">Factory that provides open MySQL connections.</param>
public class TicketRepository(IDbConnectionFactory connectionFactory) : ITicketRepository
{
    /// <summary>
    ///     Custom error raised by sp_change_ticket_status when the ticket status changed in the meantime.
    /// </summary>
    private const int ConcurrentStatusChangeErrorNumber = 50409;

    /// <inheritdoc />
    public async Task<Ticket> AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var row = await connection.QuerySingleAsync<TicketRow>(StoredProcedure(
            "sp_create_ticket",
            new
            {
                p_title = ticket.Title.Value,
                p_asset_code = ticket.AssetCode.Value,
                p_description = ticket.Description.Value,
                p_reported_by = ticket.ReportedBy.Value
            },
            cancellationToken));

        return row.ToTicket();
    }

    /// <inheritdoc />
    public async Task<Ticket?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var row = await connection.QuerySingleOrDefaultAsync<TicketRow>(StoredProcedure(
            "sp_get_ticket_by_id",
            new { p_ticket_id = id },
            cancellationToken));

        return row?.ToTicket();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Ticket>> ListAsync(TicketStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var rows = await connection.QueryAsync<TicketRow>(StoredProcedure(
            "sp_get_tickets",
            new { p_status = status is null ? null : TicketStatusMapper.ToDatabaseValue(status.Value) },
            cancellationToken));

        return rows.Select(row => row.ToTicket()).ToList();
    }

    /// <inheritdoc />
    public async Task<Ticket> UpdateStatusAsync(
        Ticket ticket,
        TicketStatus expectedStatus,
        string? comment,
        PersonName changedBy,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        try
        {
            var row = await connection.QuerySingleAsync<TicketRow>(StoredProcedure(
                "sp_change_ticket_status",
                new
                {
                    p_ticket_id = ticket.Id,
                    p_expected_status = TicketStatusMapper.ToDatabaseValue(expectedStatus),
                    p_new_status = TicketStatusMapper.ToDatabaseValue(ticket.Status),
                    p_diagnosis = ticket.Diagnosis,
                    p_comment = comment,
                    p_changed_by = changedBy.Value
                },
                cancellationToken));

            return row.ToTicket();
        }
        catch (MySqlException ex) when (ex.Number == ConcurrentStatusChangeErrorNumber)
        {
            throw new TicketConcurrencyException(ticket.Id);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TicketHistoryEntry>> FindHistoryByTicketIdAsync(int ticketId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var rows = await connection.QueryAsync<TicketHistoryRow>(StoredProcedure(
            "sp_get_ticket_history",
            new { p_ticket_id = ticketId },
            cancellationToken));

        return rows.Select(row => row.ToEntry()).ToList();
    }

    private static CommandDefinition StoredProcedure(string name, object parameters,
        CancellationToken cancellationToken) =>
        new(name, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
}