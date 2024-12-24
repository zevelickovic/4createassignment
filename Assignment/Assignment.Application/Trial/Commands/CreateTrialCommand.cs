using Assignment.Application.Interfaces;

namespace Assignment.Application.Trial.Commands;

public class CreateTrialCommand(ITrialRepository repository) : ICreateTrialCommand
{
    public async Task<int> ExecuteAsync(Domain.Entities.Trial model)
    {
        repository.Insert(model);
        return await repository.SaveChangesAsync();
    }
}