namespace MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the name of a person (the reporter or the operator).
/// </summary>
public sealed record PersonName
{
    /// <summary>
    ///     Maximum length, matching the reported_by and changed_by columns.
    /// </summary>
    public const int MaxLength = 80;

    /// <summary>
    ///     Initializes a new <see cref="PersonName"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw name. Must be non-empty and at most 80 characters.</param>
    /// <exception cref="ArgumentException">Thrown when the value is empty or too long.</exception>
    public PersonName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Person name cannot be null, empty, or whitespace.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Person name cannot be longer than {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
}