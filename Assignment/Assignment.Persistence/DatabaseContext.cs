using Assignment.Domain.Common;
using Assignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    public DatabaseContext()
    {
    }
    public virtual DbSet<Trial> Trial { get; set; }
    public virtual DbSet<TrialJsonSchema> TrialJsonSchema { get; set; }
    public new DbSet<T> Set<T>() where T : class, IEntity
    {
        return base.Set<T>();
    }
    public void Save()
    {
        SaveChanges();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("SqlServer");
        }
    }
}