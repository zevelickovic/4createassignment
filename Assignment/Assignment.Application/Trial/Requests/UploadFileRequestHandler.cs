using Assignment.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Assignment.Application.Models;
using Assignment.Application.Trial.Commands;

namespace Assignment.Application.Trial.Requests;

public class UploadFileRequestHandler(
    IValidator<UploadFileRequest> commandValidator,
    IObjectBuilder objectBuilder,
    ICreateTrialCommand command,
    ILogger<UploadFileRequestHandler> logger) : IRequestHandler<UploadFileRequest, IResult>
{
    public async Task<IResult> Handle(UploadFileRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ErrorHandler.Handle(logger,
                new ValidationResultWrapper<UploadFileRequest>(validationResult), request);
        }

        try
        {
            var trial = await objectBuilder.BuildFromFile<Domain.Entities.Trial>(request.File, cancellationToken);
            if (trial == null)
                return new ErrorResult { Message = "Internal server error, unexpected code: 11000" };

            trial.ApplyBusinessRules();

            await command.ExecuteAsync(trial);
        }
        catch (Exception e)
        {
            var exResult = new Result
            {
                IsSuccess = false,
                Message = e.Message,
                Exception = e
            };
            return await Task.FromResult(exResult);
        }

        return await Task.FromResult(new Result { IsSuccess = true });
    }
}