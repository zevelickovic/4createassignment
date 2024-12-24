namespace Assignment.Application.Models;

public record LogItem<T>
{
    public bool IsSuccess { get; init; }

    public string Message { get; init; }

    public T Command { get; init; }
}