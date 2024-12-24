using Assignment.Application.Interfaces;

namespace Assignment.Application.Trial.Queries;

public class GetTrialByTrialIdQuery(ITrialRepository repository) : IGetTrialByTrialIdQuery
{
    public async Task<Domain.Entities.Trial?> ExecuteAsync(string trialId)
    {
        return await repository.FindAsync(t => t.TrialId.Equals(trialId));
    }
}