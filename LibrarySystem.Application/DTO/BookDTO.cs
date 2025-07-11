namespace LibrarySystem.Application.DTO;

public class BookDTO
{
    public int Id { get; set; }  // For updates
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
}
