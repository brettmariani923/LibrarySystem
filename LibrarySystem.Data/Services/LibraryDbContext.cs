using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Services;

public class LibraryDbContext : DbContext //acts as a bridge between c# objects and database tables
//This class inherits from EF Core’s DbContext, which does all the heavy lifting. it manages things like databse connections, tracking entity changes, translating C# objects to sql queries, and applying migrations and schema configurations
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options) { }
    // The constructor takes DbContextOptions<LibraryDbContext> which contains configuration information like the database provider and connection string.
    // passes those options to the base DbContext constructor, options is set up in servce registration
    public DbSet<Book> Books => Set<Book>();
    // DbSet<T> represents a table in the database
    // Set<Book>() returns EF Core’s internal tracking object for that table.
    //so setting this up allows us to use _context.Books in our services to query and manipulate the Books table in the database
    protected override void OnModelCreating(ModelBuilder modelBuilder) // This method lets you customize how EF Core maps your entities to the database schema.It’s called automatically when the model is being created.
    {
        modelBuilder.Entity<Book>(b => //configures how book entity maps to the books table
        {
            b.HasKey(x => x.Id); 
            b.Property(x => x.Title).IsRequired().HasMaxLength(200); 
            b.Property(x => x.Author).IsRequired().HasMaxLength(100); 
            b.Property(x => x.Genre).HasMaxLength(50); 
            b.Property(x => x.PublishedYear).IsRequired(); 
        });
    }
}

