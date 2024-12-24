using Assignment.Application.Interfaces;
using Assignment.Domain.Entities;
using LinqKit;

namespace Assignment.Application.Trial.Queries;

public class GetTrailsByFilter(ITrialRepository repository) : IGetTrailsByFilter
{
    public async Task<IEnumerable<Domain.Entities.Trial>> ExecuteAsync(string? trialId = null, string? title = null, TrialStatus? status = null)
    {
        var predicate = PredicateBuilder.New<Domain.Entities.Trial>(true);

        if (!string.IsNullOrEmpty(trialId))
            predicate = predicate.And(trial => trial.TrialId == trialId);

        if (!string.IsNullOrEmpty(title))
            predicate = predicate.And(trial => trial.Title.Contains(title));

        if (status.HasValue)
            predicate = predicate.And(trial => trial.Status == status.Value);

        return await repository.WhereAsync(predicate);
    }
}