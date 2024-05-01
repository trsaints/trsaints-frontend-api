using trsaints_frontend_api.Context;
using trsaints_frontend_api.Mappings;
using trsaints_frontend_api.Repositories;
using trsaints_frontend_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?.Replace("{Host}", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
    .Replace("{Database}", Environment.GetEnvironmentVariable("POSTGRES_DB"))
    .Replace("{User}", Environment.GetEnvironmentVariable("POSTGRES_USER"))
    .Replace("{Password}", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

builder.Services.AddScoped<ITechStackRepository, TechStackRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();

builder.Services.AddAutoMapper(typeof(DomainToDtoProfile).Assembly);

// Configure logging
builder.Logging.ClearProviders(); // Remove all other logging providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Set the minimum log level to Debug

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
