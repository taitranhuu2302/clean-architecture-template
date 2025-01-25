using System.Text.Json;
using CleanArchitecture.API;
using CleanArchitecture.API.Extensions;
using CleanArchitecture.API.Middleware;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApi();

builder.Services.AddCors(opt => opt.AddPolicy("webApi", c =>
{
	c.AllowAnyOrigin();
	c.AllowAnyMethod();
	c.AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseCustomSwagger();
}

app.UseHealthChecks("/health");
app.UseCors("webApi");
app.UseHttpsRedirection();
app.MapEndpoints();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.InitialiseDatabaseAsync().Wait();
app.Run();