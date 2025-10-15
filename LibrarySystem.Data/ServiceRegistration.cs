using LibrarySystem.Application.Interface;
using Microsoft.EntityFrameworkCore; // brings in needed methods like usesqllite(), useinmemorydatabase, adddbcontext<T>(), etc
using Microsoft.Extensions.DependencyInjection; //lets us use IServiceCollection, which is ASP.NET cores DI container that stores all registered services

namespace LibrarySystem.Data.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool useInMemory = false)
    //This method is an extension method for IServiceCollection, allowing us to register services related to the data layer
    //The this keyword in the first parameter indicates that this is an extension method for IServiceCollection, using it for DI
    //all of this just basically makes it easy for us to register our data layer services in the API project
    {
        if (useInMemory) //if useinmemory is true, use an in-memory database for testing
        {
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        }
        else //otherwise, use sqlite with the provided connection string
        {
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlite(connectionString));
        }

        services.AddScoped<IBookService, BookService>();

        return services;
    }
}

//Here we are setting it up so that EF Core will create and manage the LibraryDbContext for us automatically, so we don’t have to manage database connections manually.


//About IServiceCollection:
//IServiceCollection is ASP.NET Core's built-in dependency injection (DI) container that stores all registered services.
//IServiceCollection is acting as a return type, allowing method chaining when configuring services in the application startup.
//It says "After adding my infrastructure services, I'll hand back the same IServiceCollection so you can keep chaining more methods"

