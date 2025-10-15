using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Services;

public class LibraryDbContext : DbContext
//Defines a custom database context class that inherits from EF core's DbContext class
//DbContext is the central class responsible for managing databse connections, tracking entity changes, translating C# objects to sql queries, and applying migrations and schema configurations
//LibraryDbContext represents the database for this application
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options) { }
    // The constructor takes DbContextOptions<LibraryDbContext> which contains configuration information like the database provider and connection string.
    // passes those options to the base DbContext constructor
    public DbSet<Book> Books => Set<Book>();
    // DbSet<T> represents a table in the database and allows you to query and save instances of T, where T is an entity type.
    // Each book object corresponds to a row in the Books table.
    // EF Core uses this property to perform CRUD operations
    // => Set<Boo>() syntax uses expression bodied syntax to return the DbSet managed by the context
    protected override void OnModelCreating(ModelBuilder modelBuilder) // This method lets you customize how EF Core maps your entities to the database schema.It’s called automatically when the model is being created.
    {
        modelBuilder.Entity<Book>(b => //configures how book entity maps to the books table
        {
            b.HasKey(x => x.Id); //defines the primary key column (Id)
            b.Property(x => x.Title).IsRequired().HasMaxLength(200); //title
            b.Property(x => x.Author).IsRequired().HasMaxLength(100); //author
            b.Property(x => x.Genre).HasMaxLength(50); //genre
            b.Property(x => x.PublishedYear).IsRequired(); //published year
        });
    }
}
