using LibrarySystem.Data.Services;
using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Application.DTO;

namespace LibrarySystem.Tests.BookTests;

public class BookServiceTests
{
    private async Task<LibraryDbContext> GetInMemoryDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new LibraryDbContext(options);

        context.Books.AddRange(
            new Book { Title = "1984", Author = "Orwell", Genre = "Dystopian", PublishedYear = 1949 },
            new Book { Title = "Dune", Author = "Herbert", Genre = "Sci-Fi", PublishedYear = 1965 }
        );
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBooks()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateAsync_AddsBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);

        var newBook = new BookDTO
        {
            Title = "The Hobbit",
            Author = "Tolkien",
            Genre = "Fantasy",
            PublishedYear = 1937
        };

        var created = await service.CreateAsync(newBook);

        Assert.True(created.Id > 0);
        Assert.Equal("The Hobbit", created.Title);
    }

    [Fact]
    public async Task DeleteAsync_RemovesBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);

        var existing = context.Books.First();
        var result = await service.DeleteAsync(existing.Id);

        Assert.True(result);
        Assert.Null(await context.Books.FindAsync(existing.Id));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);

        var existing = context.Books.First();

        var updated = new BookDTO
        {
            Id = existing.Id,
            Title = "New Title",
            Author = "New Author",
            Genre = "Updated",
            PublishedYear = 2023
        };

        var result = await service.UpdateAsync(existing.Id, updated);

        Assert.True(result);
        var updatedBook = await context.Books.FindAsync(existing.Id);
        Assert.Equal("New Title", updatedBook?.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);
        var existing = context.Books.First();
        var result = await service.GetByIdAsync(existing.Id);
        Assert.NotNull(result);
        Assert.Equal(existing.Title, result?.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullForNonExistentBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);
        var result = await service.GetByIdAsync(999); // Non-existent ID
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ThrowsExceptionForNullBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsExceptionForNonExistentBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);
        var nonExistentBook = new BookDTO
        {
            Id = 999, // Non-existent ID
            Title = "Non Existent",
            Author = "Unknown",
            Genre = "Unknown",
            PublishedYear = 2023
        };
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(999, nonExistentBook));
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseForNonExistentBook()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);
        var result = await service.DeleteAsync(999); // Non-existent ID
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyListWhenNoBooks()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new LibraryDbContext(options);
        var service = new BookService(context);
        var result = await service.GetAllAsync();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenBookNotFound()
    {
        var context = await GetInMemoryDbContextAsync();
        var service = new BookService(context);
        var result = await service.GetByIdAsync(999);
        Assert.Null(result);
    }
}