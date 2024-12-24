namespace Assignment.Application.Trial.Commands;

public interface ICreateTrialCommand
{
    Task<int> ExecuteAsync(Domain.Entities.Trial model);
}