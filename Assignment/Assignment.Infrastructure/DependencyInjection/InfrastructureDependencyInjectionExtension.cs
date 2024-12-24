using Assignment.Application.Interfaces;
using Assignment.Application.Options;
using Assignment.Infrastructure.Builders;
using Assignment.Infrastructure.TrialValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Infrastructure.DependencyInjection;

public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddOptions<TrialFileValidationOptions>().Bind(config: configuration.GetSection(nameof(TrialFileValidationOptions)));
        service.AddScoped<ITrialFileValidator, TrialFileValidator>();
        service.AddScoped<IObjectBuilder, ObjectBuilder>();
    }
}