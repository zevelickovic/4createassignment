using System.Text;
using Assignment.Application.Options;
using Assignment.Application.Trial.Queries;
using Assignment.Domain.Entities;
using Assignment.Infrastructure.TrialValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Assignment.Tests.Infrastructure
{
    [TestFixture]
    public class TrialFileValidatorTests
    {
        private Mock<IGetTrialJsonSchema> _mockSchemaQuery;
        private Mock<IOptions<TrialFileValidationOptions>> _mockOptions;
        private TrialFileValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _mockSchemaQuery = new Mock<IGetTrialJsonSchema>(MockBehavior.Strict);
            _mockOptions = new Mock<IOptions<TrialFileValidationOptions>>();
            _mockOptions.Setup(o => o.Value).Returns(new TrialFileValidationOptions { SizeLimit = 2 });
            _validator = new TrialFileValidator(_mockSchemaQuery.Object, _mockOptions.Object);
        }

        [Test]
        public async Task ValidateFileJsonSchemaAsync_ValidJson_ReturnsSuccess()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "{\"name\":\"test\"}";
            var fileName = "test.json";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);

            var schemaJson = new TrialJsonSchema { Schema = "{\"type\":\"object\",\"properties\":{\"name\":{\"type\":\"string\"}}}" };
            _mockSchemaQuery.Setup(s => s.ExecuteAsync()).ReturnsAsync(schemaJson);

            // Act
            var result = await _validator.ValidateFileJsonSchemaAsync(fileMock.Object, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task ValidateFileJsonSchemaAsync_InvalidJson_ReturnsFailure()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "{\"name\":123}";
            var fileName = "test.json";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);

            var schemaJson = new TrialJsonSchema { Schema = "{\"type\":\"object\",\"properties\":{\"name\":{\"type\":\"string\"}}}" };
            _mockSchemaQuery.Setup(s => s.ExecuteAsync()).ReturnsAsync(schemaJson);

            // Act
            var result = await _validator.ValidateFileJsonSchemaAsync(fileMock.Object, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotEmpty(result.Message);
        }

        [Test]
        public async Task ValidateFileLengthAsync_ValidLength_ReturnsSuccess()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.Length).Returns(1 * 1024 * 1024); // 1MB

            // Act
            var result = await _validator.ValidateFileLengthAsync(fileMock.Object, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task ValidateFileLengthAsync_InvalidLength_ReturnsFailure()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.Length).Returns(3 * 1024 * 1024); // 3MB

            // Act
            var result = await _validator.ValidateFileLengthAsync(fileMock.Object, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Your file exceeds the maximum allowed size of 2MB. Please upload a file that is 2MB or smaller.", result.Message);
        }

        [Test]
        public async Task ValidateFileExtensionAsync_ValidExtension_ReturnsSuccess()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("test.json");

            // Act
            var result = await _validator.ValidateFileExtensionAsync(fileMock.Object, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task ValidateFileExtensionAsync_InvalidExtension_ReturnsFailure()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("test.txt");

            // Act
            var result = await _validator.ValidateFileExtensionAsync(fileMock.Object, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("The uploaded file is not a valid JSON file. Please upload a file with a .json extension.", result.Message);
        }
    }
}
