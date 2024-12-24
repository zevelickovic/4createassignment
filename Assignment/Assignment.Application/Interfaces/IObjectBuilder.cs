using Microsoft.AspNetCore.Http;

namespace Assignment.Application.Interfaces;

public interface IObjectBuilder
{
    Task<T?> BuildFromFile<T>(IFormFile file, CancellationToken cancellationToken);
}