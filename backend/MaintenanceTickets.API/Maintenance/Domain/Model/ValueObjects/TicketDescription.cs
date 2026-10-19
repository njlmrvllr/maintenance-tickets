namespace MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a validated ticket description.
/// </summary>
public sealed record TicketDescription
{
    /// <summary>
    ///     Maximum length, matching the tickets.description column.
    /// </summary>
    public const int MaxLength = 1000;

    /// <summary>
    ///     Initializes a new <see cref="TicketDescription"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw description. Must be non-empty and at most 1000 characters.</param>
    /// <exception cref="ArgumentException">Thrown when the value is empty or too long.</exception>
    public TicketDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Ticket description cannot be null, empty, or whitespace.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Ticket description cannot be longer than {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
}