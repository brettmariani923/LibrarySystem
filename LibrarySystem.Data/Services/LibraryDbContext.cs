using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Services;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Author).IsRequired().HasMaxLength(100);
            b.Property(x => x.Genre).HasMaxLength(50);
            b.Property(x => x.PublishedYear).IsRequired();
        });
    }
}


//In this class we are basically configuring the Book entity.
//Here we're telling EF what the primary key is (Id), which fields are required or optional, and what the max lengths are for each string column. 
//This is called Fluent API configuration, and its useful when you want more control than data annotations (eg. [Required], [MaxLength]).

//It's a class that talks to the database.
//It tells Entity Framework Core(EF Core) how your data should be stored and structured in the database.

//In this case, it's used to store books in your library system.

//This class inherits from EF Core's DbContext

//That means it represents a session with the database

//You use it to read/write data

//The constructor is where EF Core passes in database settings (options)

//This is how the DbContext knows whether you're using SQLite, in-memory DB, etc.

//public DbSet<Book> Books => Set<Book>();

//This represents the Books table in the database

//EF core uses thsi to track and update book records

//protected override void OnModelCreating(ModelBuilder modelBuilder)

//This method is where you tell EF how to shape your table — like setting column names, lengths, and rules.

/*
modelBuilder.Entity<Book>(b =>
{
    b.HasKey(x => x.Id); // Id is the primary key

    b.Property(x => x.Title)
     .IsRequired()
     .HasMaxLength(200); // Title must exist, and be max 200 characters

b.Property(x => x.Author)
     .IsRequired()
     .HasMaxLength(100); // Same for Author

b.Property(x => x.Genre)
     .HasMaxLength(50); // Genre is optional but has a max length

b.Property(x => x.PublishedYear)
     .IsRequired(); // Must have a published year
});

So to sum it up, The entity class in the domain layer defines the shape of the data,
the librarydbcontext class tells ef core to create a table called books in the database, and store book objects in it using this line
public DbSet<Book> Books => Set<Book>();
And the Onmodelcreating configuration method tells ef core how to structure the table and its columns.

When you run a migration (with dotnet ef migrations add InitialCreate), EF Core generates a sql table for you like this

CREATE TABLE Books (
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL,
    Author TEXT NOT NULL,
    Genre TEXT,
    PublishedYear INTEGER NOT NULL
);

EF creates this SQL based on 
The book class
the dbcontext configuration
and the onmodelcreating method

so when you run dotnet ef database update, EF applies that sql to your real database (SQLite, SQL server, etc)
*/