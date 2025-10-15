using LibrarySystem.Application.DTO;
using LibrarySystem.Application.Interface;
using LibrarySystem.Domain.Entities;
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

    // Books == DbSet<Book> representing the Books table in the database
    // Book == EF entity that represents the actual database row
    // BookDTO == Data Transfer Object used to transfer book data between layers of the application
    public async Task<BookDTO?> CreateAsync(BookDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "Book DTO cannot be null");

        var exists = await _context.Books
            .AnyAsync(b => b.Title == dto.Title && b.Author == dto.Author);

        if (exists)
            return null; 

        var book = new Book //Book == EF entity that represents the actual database row. here we are creating the actual database table row
        {
            Title = dto.Title, //map dtos on to new row
            Author = dto.Author,
            Genre = dto.Genre,
            PublishedYear = dto.PublishedYear
        };

        _context.Books.Add(book); //adds the new book entity to the DbSet, marking it for insertion into the database when SaveChangesAsync is called
        await _context.SaveChangesAsync(); // when we call savechangesasync(), ef core generates and executes an INSERT INTNO BOOKS (...) values (...) sql command
        //if we tried to pass the dto directly, ef core wouldn't know what to do with it because its not part of the dbcontext model
        dto.Id = book.Id; //after saving changes, we assign the generated id back to the dto so the caller knows the id of the newly created book
        return dto; //return the dto with the new id
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
