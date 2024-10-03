using System.Reflection;
using Application;
using Domain;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API",
        Version = "v1",
        Description = "Password API",
    });

    options.EnableAnnotations();
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

builder.Services.AddAplicationServices();
builder.Services.AddDomainServices();

builder.Services.AddHealthChecks()
    .AddCheck("API_Health_Check", () =>
    {
        return HealthCheckResult.Healthy("API is running.");
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Password API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
