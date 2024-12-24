using Assignment.Application.Interfaces;
using FluentValidation;

namespace Assignment.Application.Trial.Requests;

public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    private readonly ITrialFileValidator _validator;

    public UploadFileRequestValidator(ITrialFileValidator validator)
    {
        _validator = validator;

        RuleFor(command => command.File)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("No file was uploaded. Please select a file to upload.")
            .NotEmpty().WithMessage("No file was uploaded. Please select a file to upload.")
            .MustAsync(async (file, cancellationToken) =>
            {
                var result = await _validator.ValidateFileLengthAsync(file, cancellationToken);
                return result.IsSuccess;
            }).WithMessage(
                "Your file exceeds the maximum allowed size.")

            .MustAsync(async (file, cancellationToken) =>
            {
                var result = await _validator.ValidateFileExtensionAsync(file, cancellationToken);
                return result.IsSuccess;
            }).WithMessage("File content does not conform to the required JSON schema.")

            .MustAsync(async (file, cancellationToken) =>
            {
                var result = await _validator.ValidateFileJsonSchemaAsync(file, cancellationToken);
                return result.IsSuccess;
            }).WithMessage("File content does not conform to the required JSON schema.");

    }
}