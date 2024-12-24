using Assignment.Domain.Common;

namespace Assignment.Domain.Entities;

public class TrialJsonSchema : IEntity
{
    public int Id { get; set; }
    public required string Schema { get; set; }
}