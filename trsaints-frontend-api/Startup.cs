using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using trsaints_frontend_api.Authorization.Middleware;
using trsaints_frontend_api.Constants;
using trsaints_frontend_api.Data;
using trsaints_frontend_api.Data.Context;
using trsaints_frontend_api.Data.Entities;
using trsaints_frontend_api.Data.Repositories;
using trsaints_frontend_api.Data.Repositories.Interfaces;
using trsaints_frontend_api.Services;
using trsaints_frontend_api.Services.Interfaces;

namespace trsaints_frontend_api;

public static class Startup
{
   public static void SetAllowedHosts(WebApplicationBuilder builder)
   {
      builder.Configuration["AllowedHosts"] =
         builder.Configuration.GetValue<string>(AllowedDomainConstants.DomainHostNames);
   }
   public static void AddCors(WebApplicationBuilder builder)
   {
      builder.Services.AddCors(options =>
      {
         var allowedDomains = builder.Configuration.GetValue<string>(AllowedDomainConstants.DomainCorsHostNames)
            .Split(";", StringSplitOptions.RemoveEmptyEntries);
         
         options.AddPolicy(AllowedDomainConstants.DomainPolicy,
            policy =>
            {
               policy.WithOrigins(allowedDomains)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            });
      });
   }

   public static void AddControllers(WebApplicationBuilder builder)
   {
      builder.Services.AddControllers()
         .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

   }

   public static void AddSwagger(WebApplicationBuilder builder)
   {
      builder.Services.AddSwaggerGen(c =>
      {
         c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trsaints API", Version = "v0.4.0" });

         // Define the BearerAuth scheme
         c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
         {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer" 
         });

         c.AddSecurityRequirement(new OpenApiSecurityRequirement
         {
            {
               new OpenApiSecurityScheme
               {
                  Reference = new OpenApiReference
                  {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                  }
               },
               Array.Empty<string>()
            }
         });
      });
   }
   
   private static string? GetFormattedConnectionString(WebApplicationBuilder builder)
   {
      var formattedConnectionString = builder.Configuration.GetConnectionString(DatabaseAccessConstants.ConnectionString)
         ?.Replace("{Host}", builder.Configuration.GetValue<string>(DatabaseAccessConstants.ConnectionHost))
         .Replace("{Port}", builder.Configuration.GetValue<string>(DatabaseAccessConstants.ConnectionPort))
         .Replace("{Database}", builder.Configuration.GetValue<string>(DatabaseAccessConstants.ConnectionDatabase))
         .Replace("{User}", builder.Configuration.GetValue<string>(DatabaseAccessConstants.ConnectionUsername))
         .Replace("{Password}", builder.Configuration.GetValue<string>(DatabaseAccessConstants.ConnectionPassword));

      return formattedConnectionString;
   }

   public static void AddDbContext(WebApplicationBuilder builder)
   {
      var connectionString = GetFormattedConnectionString(builder);

      builder.Services.AddDbContext<AppDbContext>(options =>
         options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
   }

   public static void AddScopes(WebApplicationBuilder builder)
   {
      builder.Services.AddScoped<ITechStackRepository, TechStackRepository>();
      builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
      builder.Services.AddScoped<ISkillRepository, SkillRepository>();
      builder.Services.AddScoped<ITechStackService, TechStackService>();
      builder.Services.AddScoped<IValidationService, ValidationService>();
   }

   public static void AddAuthentication(WebApplicationBuilder builder)
   {
      var jwtIssuer = builder.Configuration["Jwt:Issuer"]
         .Replace("{JwtIssuer}", builder.Configuration.GetValue<string>(JwtAuthenticationConstants.JwtIssuer));
      var jwtAudience = builder.Configuration["Jwt:Audience"]
         .Replace("{JwtAudience}", builder.Configuration.GetValue<string>(JwtAuthenticationConstants.JwtAudience));
      var jwtAuthKey = builder.Configuration["Jwt:Key"]
         .Replace("{JwtIssuerSigningKey}", builder.Configuration.GetValue<string>(JwtAuthenticationConstants.JwtIssuerSigningKey));
      
      builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
      {
         options.TokenValidationParameters = new TokenValidationParameters
         {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthKey))
         };
      })
      .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyDefaults.AuthenticationScheme, null);
   }
   
   public static void AddAuthorization(WebApplicationBuilder builder)
   {
       builder.Services.AddAuthorizationBuilder()
           .AddPolicy(ApiKeyDefaults.AuthenticationPolicy, policy =>
           {
               policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyDefaults.AuthenticationScheme);
               policy.RequireAuthenticatedUser();
           });
   }

   public static void AddIdentity(WebApplicationBuilder builder)
   {
      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
         .AddEntityFrameworkStores<AppDbContext>()
         .AddDefaultTokenProviders();
   }
   
   public static async Task EnsureCreatedRoles(RoleManager<IdentityRole> roleManager)
   {
      var roles = new List<string>
      {
         ResourceOperationsConstants.RoleAdministrators,
         ResourceOperationsConstants.RoleUsers
      };

      foreach (var role in roles)
      {
         if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
      }
   }
   
   public static void SeedDb(WebApplication app)
   {
      using var scope = app.Services.CreateScope();
      
      var serviceProvider = scope.ServiceProvider;
      var context = serviceProvider.GetRequiredService<AppDbContext>();
      context.Database.Migrate();
      
      var email = app.Configuration.GetValue<string>(DefaultUserConstants.UserName);
      var password = app.Configuration.GetValue<string>(DefaultUserConstants.UserPassword);
      
      SeedData.InitializeAsync(serviceProvider, email, password).Wait();
   }
}
