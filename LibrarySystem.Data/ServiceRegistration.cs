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
