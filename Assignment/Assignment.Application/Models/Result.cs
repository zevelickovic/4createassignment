namespace Assignment.Application.Models;

public record Result : IResult
{
    public bool IsSuccess { get; init; } = false;

    public string Message { get; init; }

    public Exception? Exception { get; init; }
}