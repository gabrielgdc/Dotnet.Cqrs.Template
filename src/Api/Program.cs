using Api.Factories;
using Api.Filters;
using Application.Common;
using FluentValidation;
using Genial.Arquitetura.LoggerActionAPI.Extensions;
using Infra.CrossCutting.Ioc.Configurations;
using Infra.CrossCutting.IoC.Configurations;
using Infra.CrossCutting.IoC.Configurations.HealthCheck;
using Infra.CrossCutting.IoC.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomLocalization();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Validator<>), ServiceLifetime.Singleton);
builder.Services.AddLoggerAction(builder.Configuration);
builder.Services.AddEndpointVersioning();
builder.Services.AddLocalization();
builder.Services.AddControllers();
builder.Services.AddSwaggerSetup();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GlobalExceptionFilterAttribute>();
builder.Services.AddScoped<ICustomProblemDetailsFactory, CustomProblemDetailsFactory>();
builder.Services.AddDependencyInjectionSetup(builder.Configuration);
builder.Services.AddDatabaseSetup();
builder.Services.AddHealthCheck();

var app = builder.Build();

app.UseCors(corsBuilder =>
{
    corsBuilder.WithOrigins("*");
    corsBuilder.AllowAnyOrigin();
    corsBuilder.AllowAnyMethod();
    corsBuilder.AllowAnyHeader();
});

app.UseRouting();
app.UseCustomLocalization();
app.UseLoggerAction(builder.Configuration);

app.MapSwagger();
app.MapControllers();
app.MapHealthCheck();

app.Run();