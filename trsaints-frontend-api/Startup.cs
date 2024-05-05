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
