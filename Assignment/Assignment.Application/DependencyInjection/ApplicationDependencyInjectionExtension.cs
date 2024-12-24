using Assignment.Application.Trial.Commands;
using Assignment.Application.Trial.Queries;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Application.DependencyInjection;

public static class ApplicationDependencyInjectionExtension
{
    public static void AddApplicationServices(this IServiceCollection service)
    {
        service.AddFluentValidators();
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationDependencyInjectionExtension).Assembly));
        service.AddQueries();
        service.AddCommands();
    }

    private static void AddFluentValidators(this IServiceCollection service)
    {
        service.AddValidatorsFromAssemblyContaining(typeof(ApplicationDependencyInjectionExtension));
    }

    private static void AddQueries(this IServiceCollection service)
    {
        service.AddScoped<IGetTrialJsonSchema, GetTrialJsonSchema>();
        service.AddScoped<IGetTrialByTrialIdQuery, GetTrialByTrialIdQuery>();
        service.AddScoped<IGetTrailsByFilter, GetTrailsByFilter>();
    }

    private static void AddCommands(this IServiceCollection service)
    {
        service.AddScoped<ICreateTrialCommand, CreateTrialCommand>();
    }
}