namespace MaintenanceTickets.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the code of the affected equipment or facility (e.g. ATM-0451).
/// </summary>
public sealed record AssetCode
{
    /// <summary>
    ///     Maximum length, matching the tickets.asset_code column.
    /// </summary>
    public const int MaxLenght = 30;

    /// <summary>
    ///     Initializes a new <see cref="AssetCode"/>. The value is normalized to upper case.
    /// </summary>
    /// <param name="value">The raw asset code. Must be non-empty and at most 30 characters.</param>
    /// <exception cref="ArgumentException">Thrown when the value is empty or too long.</exception>
    public AssetCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(value));
        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length > MaxLenght)
            throw new ArgumentException($"Asset code cannot be longer than {MaxLenght} characters.", nameof(value));

        Value = normalized;
    }
    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
}