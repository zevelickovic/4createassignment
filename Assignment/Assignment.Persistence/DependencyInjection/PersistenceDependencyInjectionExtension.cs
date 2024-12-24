using Assignment.Application.Interfaces;
using Assignment.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Persistence.DependencyInjection;

public static class PersistenceDependencyInjectionExtension
{
    public static void AddPersistenceService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });
        
        service.AddRepositories();
    }

    public static void AddRepositories(this IServiceCollection service)
    {
        service.AddScoped<ITrialJsonSchemaRepository, TrialJsonSchemaRepository>();
        service.AddScoped<ITrialRepository, TrialRepository>();
    }

    public static void ApplyMigrations<TDbContext>(this IServiceScope serviceScope) where TDbContext : DbContext
    {
        using TDbContext context = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        context.Database.Migrate();
    }
}