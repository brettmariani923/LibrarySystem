using LibrarySystem.Data.Services;
using LibrarySystem.Application.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Tests.BookTests
{
    public class ProgramTests
    {
        [Fact]
        public void AddInfrastructure_RegistersServicesCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = "Data Source=library.db"; // Example connection string
            
            // Act
            services.AddInfrastructure(connectionString);
            
            // Assert
            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetService<LibraryDbContext>());
            Assert.NotNull(provider.GetService<IBookService>());
        }

    }
}
