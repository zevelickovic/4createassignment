using Assignment.Domain.Common;

namespace Assignment.Domain.Entities;

public class Trial : IEntity
{
    public int Id { get; set; }
    public required string TrialId { get; set; }
    public required string Title { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Participants { get; set; }
    public required TrialStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Duration { get; set; }

    public void ApplyBusinessRules()
    {
        if (!EndDate.HasValue)
            EndDate = StartDate.AddMonths(1);

        Duration = (EndDate.Value - StartDate).Days;
    }
}