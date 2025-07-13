using LibrarySystem.Application.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Data.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool useInMemory = false)
    {
        if (useInMemory)
        {
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        }
        else
        {
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlite(connectionString));
        }

        services.AddScoped<IBookService, BookService>();

        return services;
    }
}
//This is a static class meant for extension methods to register services (e.g., database and custom services like BookService) in the DI container.

//public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool useInMemory = false)
//This is an extension method for IServiceCollection, allowing you to call .AddInfrastructure(...) from program.cs

// The parameters are
//- services: the DI container I'm adding services to
//- connectionString: the database connection string (for SQLite or other databases)
//- useInMemory: an optional boolean flag(default false) that tells it to use an in-memory db for testing/dev

//The conditional EF Core setup basically checks if useInMemory is true, and if ot is it configures the LibraryDbContext to use an in-memory database named "TestDb".
//otherwise it just uses sqlite with the provided connection string. 
//This is for flexibility between dev/test and production environments.

//        services.AddScoped<IBookService, BookService>();
//this is for registering our services
//tells the DI container that whenever someone asks for IBookService, it should provide an instance of BookService.
//This is needed for the bookcontroller to work, as it depends on IBookService.

//return services just returns the updated IServiceCollection so that it can be chained in program.cs or wherever it's called.

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddInfrastructure(connectionString, useInMemory: false);
//these are examples of its usage in the Program.cs file


