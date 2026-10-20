namespace MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a validated ticket title.
/// </summary>


public sealed record TicketTitle
{
    /// <summary>
    ///     Maximum length, matching the tickets.title column.
    /// </summary>
    public const int MaxLength = 120;

    /// <summary>
    ///     Initializes a new <see cref="TicketTitle"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw title. Must be non-empty and at most 120 characters.</param>
    /// <exception cref="ArgumentException">Thrown when the value is empty or too long.</exception>
    public TicketTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ticket title cannot be null, empty or whitespace.", nameof(value));
        
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException("ticket title cannot be longer than " + MaxLength + " characters.", nameof(value));
        Value = trimmed;
    }
    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
    
    
    
}

    