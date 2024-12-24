using Assignment.Application.Interfaces;
using Assignment.Domain.Entities;

namespace Assignment.Persistence.Repositories;

public class TrialJsonSchemaRepository(DatabaseContext database) : Repository<TrialJsonSchema>(database), ITrialJsonSchemaRepository;