using System.Text;
using Assignment.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Assignment.Infrastructure.Builders;

public class ObjectBuilder : IObjectBuilder
{
    public async Task<T?> BuildFromFile<T>(IFormFile file, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
        
        var json = await reader.ReadToEndAsync(cancellationToken);
        
        var trial = JsonConvert.DeserializeObject<T>(json);
        
        return await Task.FromResult(trial);
    }
}