using LibrarySystem.Application.DTO;
using LibrarySystem.Application.Interface;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Services;

public class BookService : IBookService
{
    private readonly LibraryDbContext _context;

    public BookService(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookDTO>> GetAllAsync()
    {
        return await _context.Books
            .Select(book => new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                PublishedYear = book.PublishedYear
            })
            .ToListAsync();
    }

    public async Task<BookDTO?> GetByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return null;

        return new BookDTO
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear
        };
    }

    public async Task<BookDTO?> CreateAsync(BookDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Book DTO cannot be null");

        var exists = await _context.Books
            .AnyAsync(b => b.Title == dto.Title && b.Author == dto.Author);

        if (exists)
            return null; 

        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            Genre = dto.Genre,
            PublishedYear = dto.PublishedYear
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        dto.Id = book.Id;
        return dto;
    }

    public async Task<bool> UpdateAsync(int id, BookDTO DTO)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            throw new InvalidOperationException($"Book with ID {id} does not exist.");

        book.Title = DTO.Title;
        book.Author = DTO.Author;
        book.Genre = DTO.Genre;
        book.PublishedYear = DTO.PublishedYear;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}
