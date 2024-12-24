using Assignment.Domain.Entities;

namespace Assignment.Application.Trial.Queries;

public interface IGetTrailsByFilter
{
    Task<IEnumerable<Domain.Entities.Trial>> ExecuteAsync(string? trialId = null, string? title = null, TrialStatus? status = null);
}