namespace Assignment.Application.Trial.Queries;

public interface IGetTrialByTrialIdQuery
{
    Task<Domain.Entities.Trial?> ExecuteAsync(string trialId);
}