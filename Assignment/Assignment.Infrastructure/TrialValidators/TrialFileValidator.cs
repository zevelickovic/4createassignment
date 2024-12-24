using System.Text;
using Assignment.Application.Interfaces;
using Assignment.Application.Models;
using Assignment.Application.Options;
using Assignment.Application.Trial.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using IResult = Assignment.Application.Models.IResult;

namespace Assignment.Infrastructure.TrialValidators;

public class TrialFileValidator(IGetTrialJsonSchema schemaQuery, IOptions<TrialFileValidationOptions> options) : ITrialFileValidator
{
    private readonly long _sizeLimit = options.Value.SizeLimit * 1024 * 1024;

    public long SizeLimit => _sizeLimit;

    public async Task<IResult> ValidateFileJsonSchemaAsync(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                var jsonContent = await reader.ReadToEndAsync(cancellationToken);

                var schemaJson = await schemaQuery.ExecuteAsync();
                var schema = JSchema.Parse(schemaJson.Schema);

                var jsonObject = JObject.Parse(jsonContent);
                var isValid = jsonObject.IsValid(schema, out IList<string> validationErrors);
                if (isValid)
                {
                    var result = new Result { IsSuccess = true };
                    return await Task.FromResult(result);
                }

                var errorMessage = $"Validation Errors:\n - {string.Join("\n - ", validationErrors)}";
                var error = new Result
                {
                    IsSuccess = false,
                    Message = errorMessage
                };
                return await Task.FromResult(error);
            }
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
    }


    public async Task<IResult> ValidateFileLengthAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length <= _sizeLimit)
        {
            var result = new Result { IsSuccess = true };
            return await Task.FromResult(result);
        }
        var error = new Result
        {
            IsSuccess = false,
            Message = "Your file exceeds the maximum allowed size of 2MB. Please upload a file that is 2MB or smaller."
        };
        return await Task.FromResult(error);
    }

    public async Task<IResult> ValidateFileExtensionAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        if (extension.Equals(".json") || extension.Equals("json"))
        {
            var result = new Result { IsSuccess = true };
            return await Task.FromResult(result);
        }
        var error = new Result
        {
            IsSuccess = false,
            Message = "The uploaded file is not a valid JSON file. Please upload a file with a .json extension."
        };
        return await Task.FromResult(error);
    }
}