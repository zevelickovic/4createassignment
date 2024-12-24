using FluentValidation.Results;

namespace Assignment.Application.Models;

public record ValidationResultWrapper<T> : IResult
{
    public ValidationResult ValidationResult { get; init; }

    public bool IsSuccess { get; init; }

    public string Message { get; init; }

    public Exception? Exception { get; init; } = null;

    public ValidationResultWrapper(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
        IsSuccess = validationResult.IsValid;

        if (!IsSuccess)
        {
            Message = "Data validation error: ";
            foreach (var error in validationResult.Errors)
                Message += $"{error.ErrorMessage} ";
        }
    }
}