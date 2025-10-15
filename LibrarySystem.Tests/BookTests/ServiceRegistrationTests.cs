using LibrarySystem.Application.Interface;
using LibrarySystem.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibrarySystem.Tests.BookTests
{
    public class ServiceRegistrationTests
    {
        [Fact]
        public void ServiceRegistration_RegistersAllServices()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddInfrastructure(connectionString: "", useInMemory: true);

            var provider = services.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IBookService>());
            Assert.NotNull(provider.GetService<LibraryDbContext>());
            Assert.NotNull(provider.GetService<ILogger<BookService>>());
        }

        [Fact]
        public void ServiceRegistration_RegistersDbContextWithCorrectOptions()
        {
            var services = new ServiceCollection();
            services.AddInfrastructure(connectionString: "", useInMemory: true);

            var provider = services.BuildServiceProvider();
            var dbContext = provider.GetService<LibraryDbContext>();

            Assert.NotNull(dbContext);
            Assert.IsType<LibraryDbContext>(dbContext);
        }

        [Fact]
        public void ServiceRegistration_ThrowsExceptionForMissingDbContext()
        {
            var services = new ServiceCollection(); // did NOT call AddInfrastructure

            Assert.Throws<InvalidOperationException>(() =>
                services.BuildServiceProvider().GetRequiredService<LibraryDbContext>());
        }
    }
}
