using Microsoft.EntityFrameworkCore;

namespace BooksSample;

public class BooksContext : DbContext
{
    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var books = Enumerable.Range(1, 100).Select(i => new Book(i, $"sample {i}", i % 2 == 0 ? "One" : "Two"));
        modelBuilder.Entity<Book>().HasData(books);
    }

    public DbSet<Book> Books => Set<Book>();
}
