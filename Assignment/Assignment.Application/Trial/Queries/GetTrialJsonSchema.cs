using Assignment.Application.Interfaces;
using Assignment.Domain.Entities;

namespace Assignment.Application.Trial.Queries;

public class GetTrialJsonSchema(ITrialJsonSchemaRepository repository) : IGetTrialJsonSchema
{
    public async Task<TrialJsonSchema> ExecuteAsync()
    {
        return await repository.GetByIdAsync(1) ?? throw new InvalidOperationException("The required data schema is not found in the system. Please ensure that the schema exists before performing the validation.");
    }
}