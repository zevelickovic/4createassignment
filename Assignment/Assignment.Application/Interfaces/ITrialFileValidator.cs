using Microsoft.AspNetCore.Http;
using IResult = Assignment.Application.Models.IResult;

namespace Assignment.Application.Interfaces;

public interface ITrialFileValidator
{
    long SizeLimit { get; }
    Task<IResult> ValidateFileJsonSchemaAsync(IFormFile file, CancellationToken cancellationToken);
    Task<IResult> ValidateFileLengthAsync(IFormFile file, CancellationToken cancellationToken);
    Task<IResult> ValidateFileExtensionAsync(IFormFile file, CancellationToken cancellationToken);
}