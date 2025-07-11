using LibrarySystem.Application.DTO;

namespace LibrarySystem.Application.Interface;

public interface IBookService
{
    Task<IEnumerable<BookDTO>> GetAllAsync();
    Task<BookDTO?> GetByIdAsync(int id);
    Task<BookDTO> CreateAsync(BookDTO book);
    Task<bool> UpdateAsync(int id, BookDTO book);
    Task<bool> DeleteAsync(int id);
}
