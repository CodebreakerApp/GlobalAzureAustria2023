namespace BooksSample;

public interface IBooksRepository
{
    Task<Book> AddBookAsync(Book book, CancellationToken cancellation = default);
    Task<Book?> DeleteBookAsync(int bookId, CancellationToken cancellation = default);
    Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellation = default);
    Task<Book?> GetBookByIdAsync(int bookId, CancellationToken cancellation = default);
    Task<bool> UpdateBookAsync(Book book, CancellationToken cancellation = default);
}