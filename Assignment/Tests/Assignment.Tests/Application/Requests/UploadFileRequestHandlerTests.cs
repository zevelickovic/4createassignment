using Assignment.Application.Interfaces;
using Assignment.Application.Trial.Commands;
using Assignment.Application.Trial.Requests;
using Assignment.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Assignment.Tests.Application.Requests
{
    [TestFixture]
    public class UploadFileRequestHandlerTests
    {
        private Mock<IValidator<UploadFileRequest>> _mockValidator;
        private Mock<IObjectBuilder> _mockObjectBuilder;
        private Mock<ICreateTrialCommand> _mockCommand;
        private Mock<ILogger<UploadFileRequestHandler>> _mockLogger;
        private UploadFileRequestHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockValidator = new Mock<IValidator<UploadFileRequest>>();
            _mockObjectBuilder = new Mock<IObjectBuilder>();
            _mockCommand = new Mock<ICreateTrialCommand>();
            _mockLogger = new Mock<ILogger<UploadFileRequestHandler>>();
            _handler = new UploadFileRequestHandler(
                _mockValidator.Object,
                _mockObjectBuilder.Object,
                _mockCommand.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_BuildsTrialAndExecutesCommand()
        {
            // Arrange
            var mockFile = Mock.Of<IFormFile>();
            var request = new UploadFileRequest(mockFile);
            var validationResult = new ValidationResult();
            var trial = new Domain.Entities.Trial
            {
                TrialId = "dummy trial id",
                Title = "dummy title",
                StartDate = DateTime.UtcNow,
                Status = TrialStatus.NotStarted
            };
            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _mockObjectBuilder.Setup(ob => ob.BuildFromFile<Domain.Entities.Trial>(request.File, It.IsAny<CancellationToken>())).ReturnsAsync(trial);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            _mockCommand.Verify(c => c.ExecuteAsync(trial), Times.Once);
        }

        [Test]
        public async Task Handle_InvalidRequest_ReturnsErrorResult()
        {
            // Arrange
            var request = new UploadFileRequest(null);
            var validationResult = new ValidationResult(new[] { new ValidationFailure("File", "File is required") });
            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Data validation error: File is required ", result.Message);
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResult()
        {
            // Arrange
            var mockFile = Mock.Of<IFormFile>();
            var request = new UploadFileRequest(mockFile);
            var validationResult = new ValidationResult();
            _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _mockObjectBuilder.Setup(ob => ob.BuildFromFile<Domain.Entities.Trial>(request.File, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Test exception", result.Message);
        }
    }
}
