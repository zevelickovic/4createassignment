using Assignment.Domain.Entities;

namespace Assignment.Application.Trial.Queries;

public interface IGetTrialJsonSchema
{
    Task<TrialJsonSchema> ExecuteAsync();
}