using Microsoft.Extensions.Logging;

namespace Assignment.Application.Models;

public static class ErrorHandler
{
    public static IResult Handle<T>(ILogger logger, IResult result, T command)
    {
        Log(logger, result, command);
        return new ErrorResult { Message = result.Message, Exception = result.Exception };
    }

    public static void Log<T>(ILogger logger, IResult result, T command)
    {
        var logItem = new LogItem<T>
        {
            Message = result.Message,
            Command = command
        };
        logger.LogError(result.Exception, "{@ErrorMessage}", logItem);
    }
}