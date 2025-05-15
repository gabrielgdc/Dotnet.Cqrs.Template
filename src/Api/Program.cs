using Api.Factories;
using Api.Filters;
using Application.Shared.ResultTypes;
using FluentValidation;
using Infra.CrossCutting.Ioc.Configurations;
using Infra.CrossCutting.Ioc.Configurations.HealthCheck;
using Infra.CrossCutting.Ioc.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomLocalization();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Validator<>), ServiceLifetime.Singleton);
builder.Services.AddEndpointVersioning();
builder.Services.AddLocalization();
builder.Services.AddSwaggerSetup();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GlobalExceptionFilterAttribute>();
builder.Services.AddScoped<ICustomProblemDetailsFactory, CustomProblemDetailsFactory>();
builder.Services.AddDependencyInjectionSetup(builder.Configuration);
builder.Services.AddDatabaseSetup();
builder.Services.AddHealthCheck();
builder.Services.AddControllers()
       .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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

app.MapSwagger();
app.MapControllers();
app.MapHealthCheck();

app.Run();