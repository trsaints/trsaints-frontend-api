using trsaints_frontend_api.Context;
using trsaints_frontend_api.Mappings;
using Microsoft.AspNetCore.Identity;
using trsaints_frontend_api;
using trsaints_frontend_api.Entities;

var builder = WebApplication.CreateBuilder(args);

Startup.AddCors(builder);
Startup.AddControllers(builder);

builder.Services.AddEndpointsApiExplorer();

Startup.AddSwagger(builder);
Startup.AddDbContext(builder);
Startup.AddScopes(builder);

builder.Services.AddAutoMapper(typeof(DomainToDtoProfile).Assembly);

Startup.AddAuthentication(builder);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

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

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
