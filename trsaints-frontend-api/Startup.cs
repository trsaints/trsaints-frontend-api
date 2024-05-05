using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace trsaints_frontend_api;

public static class Startup
{
   public static void AddCors(WebApplicationBuilder builder)
   {
      builder.Services.AddCors(options =>
      {
         options.AddPolicy(name: "basePolicy",
            policy =>
            {
               policy.WithOrigins("https://www.trsantos.tech/", "https://localhost:8080")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            });
      });
   }

   public static void AddControllers(WebApplicationBuilder builder)
   {
      builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

   }

   public static void AddSwagger(WebApplicationBuilder builder)
   {
      builder.Services.AddSwaggerGen(options =>
      {
         options.SwaggerDoc("v1", info: new OpenApiInfo
         {
            Title = "TRSaints API",
            Version = "v1"
         });
    
         options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
         {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
         });
    
         options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
   
   public static string? GetFormattedConnectionString(WebApplicationBuilder builder)
   {
      var formattedConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
         ?.Replace("{Host}", builder.Configuration.GetValue<string>("POSTGRES_HOST"))
         .Replace("{Port}", builder.Configuration.GetValue<string>("POSTGRES_PORT"))
         .Replace("{Database}", builder.Configuration.GetValue<string>("POSTGRES_DB"))
         .Replace("{User}", builder.Configuration.GetValue<string>("POSTGRES_USER"))
         .Replace("{Password}", builder.Configuration.GetValue<string>("POSTGRES_PASSWORD"));

      return formattedConnectionString;
   } 
}
