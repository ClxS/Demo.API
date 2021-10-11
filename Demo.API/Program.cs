using Demo.API.Endpoints;
using Demo.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Demo.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "Demo.API", Version = "v1" }); });
builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var repositoryImplementation = config["RepositoryImplementation"];
if (repositoryImplementation == "EF")
{
    builder.Services.AddDbContext<DemoContext>(options => options.UseMySQL(config.GetConnectionString("Demo")));
    builder.Services.AddScoped<DbContext, DemoContext>();
}
builder.AddGuitarServices(repositoryImplementation);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.API v1"));
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/errors");

app.MapGuitarEndpoints();
app.MapErrorEndpoints();

app.Run();