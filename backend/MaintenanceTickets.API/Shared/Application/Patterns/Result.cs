namespace MaintenanceTickets.API.Shared.Application.Patterns;

/// <summary>
///     Represents the outcome of an operation that can succeed with a value or fail with an error.
/// </summary>
/// <typeparam name="TValue">The type of the successful result value.</typeparam>
/// <typeparam name="TError">The type of the error information.</typeparam>
public abstract record Result<TValue, TError>
{
    /// <summary>
    ///     Represents a successful operation outcome.
    /// </summary>
    /// <param name="Value">The resulting value from the successful operation.</param>
    public sealed record Success(TValue Value) : Result<TValue, TError>;

    /// <summary>
    ///     Represents a failed operation outcome.
    /// </summary>
    /// <param name="Error">The error information from the failed operation.</param>
    public sealed record Failure(TError Error) : Result<TValue, TError>;

    /// <summary>
    ///     Determines whether the result represents a successful outcome.
    /// </summary>
    public bool IsSuccess => this is Success;

    /// <summary>
    ///     Determines whether the result represents a failed outcome.
    /// </summary>
    public bool IsFailure => this is Failure;

    /// <summary>
    ///     Applies a function to either the success or failure case.
    /// </summary>
    public TResult Fold<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onFailure) =>
        this switch
        {
            Success s => onSuccess(s.Value),
            Failure f => onFailure(f.Error),
            _ => throw new InvalidOperationException("Unknown Result type")
        };
}