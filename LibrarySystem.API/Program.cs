using LibrarySystem.Data.Services;

namespace LibrarySystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Adds the default API explorer, which is used to generate OpenAPI documentation
            builder.Services.AddSwaggerGen();
            //scans controllers, routes, and models(boot dto) to build the OpenAPI spec


            // SQLite connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                  ?? "Data Source=library.db";
            builder.Services.AddInfrastructure(connectionString);
            // Calls the AddInfrastructure method in the Data layer(service registration) to register LibraryDbContext and other services.
            // Basically tells ASP.NET Core’s dependency injection system: 
            // "When someone asks for a LibraryDbContext, here's how to create it using this connection string."

            var app = builder.Build();

            // Run migrations automatically
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                db.Database.EnsureCreated(); // or db.Database.Migrate(); if using migrations
            }

            // Middleware
            app.UseSwagger();
            // Enables the Swagger middleware, which generates the OpenAPI documentation and serves it at /swagger endpoint
            app.UseSwaggerUI();
            //serves a web page at /swagger so I can test the api
            //reflects all endpoints like Get /api/books, Post /api/books, etc.
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
