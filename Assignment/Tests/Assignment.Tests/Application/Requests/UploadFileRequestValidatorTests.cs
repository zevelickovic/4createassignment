using Assignment.Application.Interfaces;
using Assignment.Application.Models;
using Assignment.Application.Trial.Requests;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Assignment.Tests.Application.Requests
{
    [TestFixture]
    public class UploadFileRequestValidatorTests
    {
        private Mock<ITrialFileValidator> _mockValidator;
        private UploadFileRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _mockValidator = new Mock<ITrialFileValidator>();
            _validator = new UploadFileRequestValidator(_mockValidator.Object);
        }

        [Test]
        public async Task Should_Have_Error_When_File_Is_Null()
        {
            var request = new UploadFileRequest(null);
            var result = await _validator.TestValidateAsync(request);
            result.ShouldHaveValidationErrorFor(r => r.File)
                .WithErrorMessage("No file was uploaded. Please select a file to upload.");
        }

        [Test]
        public async Task Should_Have_Error_When_File_Is_Empty()
        {
            var request = new UploadFileRequest(null);
            var result = await _validator.TestValidateAsync(request);
            result.ShouldHaveValidationErrorFor(r => r.File)
                .WithErrorMessage("No file was uploaded. Please select a file to upload.");
        }

        [Test]
        public async Task Should_Have_Error_When_File_Exceeds_Size_Limit()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(5);
            _mockValidator.Setup(v => v.ValidateFileLengthAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = false });
            _mockValidator.Setup(v => v.SizeLimit).Returns(2);
            
            var request = new UploadFileRequest(fileMock.Object);
            var result = await _validator.TestValidateAsync(request);
            result.ShouldHaveValidationErrorFor(r => r.File)
                .WithErrorMessage("Your file exceeds the maximum allowed size.");
        }

        [Test]
        public async Task Should_Have_Error_When_File_Has_Invalid_Extension()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("test.txt");
            _mockValidator.Setup(v => v.SizeLimit).Returns(2);
            _mockValidator.Setup(v => v.ValidateFileLengthAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = true });
            _mockValidator.Setup(v => v.ValidateFileExtensionAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = false });

            var request = new UploadFileRequest(fileMock.Object); ;
            var result = await _validator.TestValidateAsync(request);
            result.ShouldHaveValidationErrorFor(r => r.File)
                .WithErrorMessage("File content does not conform to the required JSON schema.");
        }

        [Test]
        public async Task Should_Have_Error_When_File_Has_Invalid_Json_Schema()
        {

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.FileName).Returns("test.json");
            _mockValidator.Setup(v => v.ValidateFileLengthAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = true });
            _mockValidator.Setup(v => v.ValidateFileExtensionAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = true });
            _mockValidator.Setup(v => v.ValidateFileJsonSchemaAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = false });

            var request = new UploadFileRequest(fileMock.Object); ;
            var result = await _validator.TestValidateAsync(request);
            result.ShouldHaveValidationErrorFor(r => r.File)
                .WithErrorMessage("File content does not conform to the required JSON schema.");
        }

        [Test]
        public async Task Should_Not_Have_Error_When_File_Is_Valid()
        {
            var fileMock = new Mock<IFormFile>();
            _mockValidator.Setup(v => v.ValidateFileLengthAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = true });
            _mockValidator.Setup(v => v.ValidateFileExtensionAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = true });
            _mockValidator.Setup(v => v.ValidateFileJsonSchemaAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { IsSuccess = true });

            var request = new UploadFileRequest(fileMock.Object); ;
            var result = await _validator.TestValidateAsync(request);
            result.ShouldNotHaveValidationErrorFor(r => r.File);
        }
    }
}
