using System.Text;
using trsaints_frontend_api.Context;
using trsaints_frontend_api.Mappings;
using trsaints_frontend_api.Repositories;
using trsaints_frontend_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using trsaints_frontend_api.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?.Replace("{Host}", builder.Configuration.GetValue<string>("POSTGRES_HOST"))
    .Replace("{Port}", builder.Configuration.GetValue<string>("POSTGRES_PORT"))
    .Replace("{Database}", builder.Configuration.GetValue<string>("POSTGRES_DB"))
    .Replace("{User}", builder.Configuration.GetValue<string>("POSTGRES_USER"))
    .Replace("{Password}", builder.Configuration.GetValue<string>("POSTGRES_PASSWORD"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

builder.Services.AddScoped<ITechStackRepository, TechStackRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();

builder.Services.AddAutoMapper(typeof(DomainToDtoProfile).Assembly);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"].Replace("{AuthKey}", builder.Configuration.GetValue<string>("JWT_AUTH_KEY"))))
    };
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
