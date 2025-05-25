using Api.Factories;
using Api.Filters;
using Application.Shared.ResultTypes;
using FluentValidation;
using Infra.CrossCutting.Ioc.Configurations;
using Infra.CrossCutting.Ioc.Configurations.HealthCheck;
using Infra.CrossCutting.Ioc.Configurations.Logging;
using Infra.CrossCutting.Ioc.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddLocalizationSetup();
builder.Services.AddSwaggerSetup();
builder.Services.AddLoggingSetup(builder.Configuration);
builder.Services.AddDependencyInjectionSetup(builder.Configuration);
builder.Services.AddDatabaseSetup();
builder.Services.AddHealthCheckSetup();
builder.Services.AddEndpointVersioningSetup();

builder.Services.AddScoped<GlobalExceptionFilterAttribute>();
builder.Services.AddScoped<ICustomProblemDetailsFactory, CustomProblemDetailsFactory>();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Validator<>), ServiceLifetime.Singleton);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(corsBuilder =>
{
    corsBuilder.AllowAnyOrigin();
    corsBuilder.AllowAnyMethod();
    corsBuilder.AllowAnyHeader();
});

app.UseRouting();
app.UseLocalization();

app.MapSwagger();
app.MapControllers();
app.MapHealthCheck();

app.Run();