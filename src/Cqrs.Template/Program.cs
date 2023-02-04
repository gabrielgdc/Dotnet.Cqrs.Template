using System.Reflection;
using Cqrs.Template.Application.CommandHandlers;
using MediatR;
using Cqrs.Template.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Cqrs.Template.Infra.CrossCutting.IoC.Configurations;
using Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Authentication;
using Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Logging;
using Cqrs.Template.Infra.CrossCutting.IoC.Configurations.HealthCheck;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddCustomLogging(builder.Configuration);
builder.Services.AddCustomAuthentication();
builder.Services.AddApiVersioning();
builder.Services.AddVersionedApiExplorer();
builder.Services.AddSwaggerSetup();
builder.Services.AddAutoMapper();
builder.Services.AddDependencyInjectionSetup(builder.Configuration);
builder.Services.AddMediatR(typeof(CommandHandler<,>));
builder.Services.AddScoped<GlobalExceptionFilterAttribute>();
builder.Services.AddDatabaseSetup();
builder.Services.AddControllers();

builder.Services.AddHealthCheck(builder.Configuration);

var app = builder.Build();

app.UseLoggingMiddlewares();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(corsBuilder =>
{
    corsBuilder.WithOrigins("*");
    corsBuilder.AllowAnyOrigin();
    corsBuilder.AllowAnyMethod();
    corsBuilder.AllowAnyHeader();
});

app.UseRouting();

app.UseAuthorization();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerSetup(apiVersionDescriptionProvider);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthCheck();
});

app.Run();
