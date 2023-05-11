using Microsoft.EntityFrameworkCore;

namespace BooksSample;

public class BooksContext : DbContext, IBooksRepository
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

    public async Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellation = default)
    {
        return await Books.ToListAsync(cancellation);
    }

    public async Task<Book?> GetBookByIdAsync(int bookId, CancellationToken cancellation = default)
    {
        return await Books.FindAsync(new object[] { bookId }, cancellation);
    }

    public async Task<bool> UpdateBookAsync(Book book, CancellationToken cancellation = default)
    {
        var found = await Books.AsNoTracking().SingleOrDefaultAsync(b => b.BookId == book.BookId, cancellation);
        if (found is null) 
            return false;

        Books.Update(book);
        await SaveChangesAsync(cancellation);
        return true;
    }

    public async Task<Book> AddBookAsync(Book book, CancellationToken cancellation = default)
    {
        Books.Add(book);
        await SaveChangesAsync(cancellation);
        return book;
    }

    public async Task<Book?> DeleteBookAsync(int bookId, CancellationToken cancellation = default)
    {
        var book = await Books.AsNoTracking().SingleOrDefaultAsync(b => b.BookId == bookId, cancellation);
        if (book is not null)
        {
            Books.Remove(book);
            await SaveChangesAsync(cancellation);
        }
        return book;
    }
}
