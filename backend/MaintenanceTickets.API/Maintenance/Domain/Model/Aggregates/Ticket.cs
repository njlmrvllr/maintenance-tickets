using MaintenanceTickets.API.Maintenance.Domain.Model.Commands;
using MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;
using MaintenanceTickets.API.Maintenance.Domain.Model.Policies;

namespace MaintenanceTickets.API.Maintenance.Domain.Model.Aggregates;

/// <summary>
///     Represents a maintenance ticket reported by a user and managed by an operator.
/// </summary>
/// <remarks>
///     This aggregate root protects the ticket workflow: its status can only change
///     through <see cref="ChangeStatus"/>, which applies the state machine rules.
/// </remarks>
public partial class Ticket
{
    /// <summary>
    ///     Maximum length of a status change comment, matching the ticket_history.comment column.
    /// </summary>
    public const int MaxCommentLength = 500;

    /// <summary>
    ///     Protected parameterless constructor for persistence.
    /// </summary>
    protected Ticket()
    {
        Title = null!;
        AssetCode = null!;
        Description = null!;
        ReportedBy = null!;
    }

    /// <summary>
    ///     Creates a new ticket from a creation command. Every new ticket starts as Pending.
    /// </summary>
    /// <param name="command">The CreateTicketCommand command.</param>
    /// <exception cref="ArgumentNullException">Thrown when command is null.</exception>
    public Ticket(CreateTicketCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        Title = command.Title;
        AssetCode = command.AssetCode;
        Description = command.Description;
        ReportedBy = command.ReportedBy;
        Status = TicketStatus.Pending;
    }

    /// <summary>Gets the server-generated identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the ticket title.</summary>
    public TicketTitle Title { get; private set; }

    /// <summary>Gets the code of the affected equipment or facility.</summary>
    public AssetCode AssetCode { get; private set; }

    /// <summary>Gets the incident description.</summary>
    public TicketDescription Description { get; private set; }

    /// <summary>Gets the current status.</summary>
    public TicketStatus Status { get; private set; }

    /// <summary>Gets the diagnosis, set when the ticket is resolved.</summary>
    public string? Diagnosis { get; private set; }

    /// <summary>Gets the name of the person who reported the incident.</summary>
    public PersonName ReportedBy { get; private set; }

    /// <summary>
    ///     Tries to move the ticket to a new status, applying the state machine rules.
    /// </summary>
    /// <param name="targetStatus">The status to move the ticket to.</param>
    /// <param name="comment">Diagnosis when resolving, reason when cancelling, or an optional note.</param>
    /// <returns>
    ///     <see cref="TicketTransitionOutcome.Accepted"/> if the status changed;
    ///     otherwise the reason it was rejected. A rejected transition leaves the ticket unchanged.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the comment is longer than 500 characters.</exception>
    public TicketTransitionOutcome ChangeStatus(TicketStatus targetStatus, string? comment)
    {
        if (comment is { Length: > MaxCommentLength })
            throw new ArgumentException($"Comment cannot be longer than {MaxCommentLength} characters.", nameof(comment));

        if (!TicketStateMachine.CanTransition(Status, targetStatus))
            return TicketTransitionOutcome.InvalidTransition;

        var hasComment = !string.IsNullOrWhiteSpace(comment);

        if (targetStatus == TicketStatus.Resolved && !hasComment)
            return TicketTransitionOutcome.DiagnosisRequired;

        if (targetStatus == TicketStatus.Cancelled && !hasComment)
            return TicketTransitionOutcome.ReasonRequired;

        Status = targetStatus;
        if (targetStatus == TicketStatus.Resolved)
            Diagnosis = comment!.Trim();

        return TicketTransitionOutcome.Accepted;
    }

    /// <summary>
    ///     Rebuilds a ticket that already exists in the database.
    /// </summary>
    /// <remarks>
    ///     Used only by the infrastructure layer when reading from the database.
    ///     It does not apply business rules because the data was already validated when it was saved.
    /// </remarks>
    public static Ticket Restore(
        int id,
        TicketTitle title,
        AssetCode assetCode,
        TicketDescription description,
        TicketStatus status,
        string? diagnosis,
        PersonName reportedBy,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt) =>
        new()
        {
            Id = id,
            Title = title,
            AssetCode = assetCode,
            Description = description,
            Status = status,
            Diagnosis = diagnosis,
            ReportedBy = reportedBy,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
}