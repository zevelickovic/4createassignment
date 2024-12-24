using Assignment.Application.Interfaces;
using Assignment.Domain.Entities;

namespace Assignment.Persistence.Repositories;

public class TrialRepository(DatabaseContext database) : Repository<Trial>(database), ITrialRepository;