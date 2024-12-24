using Assignment.Application.DependencyInjection;
using Assignment.Infrastructure.DependencyInjection;
using Assignment.Persistence;
using Assignment.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using IServiceScope serviceScope = app.Services.CreateScope();
    serviceScope.ApplyMigrations<DatabaseContext>();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();