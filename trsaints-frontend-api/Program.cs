using Microsoft.AspNetCore.Authorization;
using trsaints_frontend_api.Mappings;
using trsaints_frontend_api;
using trsaints_frontend_api.Authorization;

var builder = WebApplication.CreateBuilder(args);

Startup.AddCors(builder);
Startup.AddControllers(builder);

builder.Services.AddEndpointsApiExplorer();

Startup.AddSwagger(builder);
Startup.AddDbContext(builder);
Startup.AddScopes(builder);

builder.Services.AddAutoMapper(typeof(DomainToDtoProfile).Assembly);

Startup.AddAuthentication(builder);
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IAuthorizationHandler, ResourceAdministratorsAuthorizationHandler>();

Startup.AddIdentity(builder);

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

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
