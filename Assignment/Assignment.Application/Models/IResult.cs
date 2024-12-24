namespace Assignment.Application.Models;

public interface IResult
{
    public bool IsSuccess { get; init; }

    public string Message { get; init; }

    public Exception? Exception { get; init; }
}