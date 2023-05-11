using System.Collections.Concurrent;

namespace BooksSample;

public class BooksService : IBooksRepository
{
    private readonly ConcurrentDictionary<int, Book> _books;
    private readonly object _addLock = new();

    public BooksService()
    {
        var books = 
            Enumerable.Range(0, 100)
                .Select(i => 
                    new KeyValuePair<int, Book>(i, new Book(i, $"title {i}", "pub")));
        _books = new ConcurrentDictionary<int, Book>(books);
    }

    public Task<Book> AddBookAsync(Book book, CancellationToken cancellation = default)
    {
        lock (_addLock)
        {
            int nextKey = _books.Max(b => b.Key) + 1;
            book = book with { BookId = nextKey };
            _books.TryAdd(book.BookId, book);
            return Task.FromResult(book);
        }
    }

    public Task<Book?> DeleteBookAsync(int bookId, CancellationToken cancellation = default)
    {
        _books.TryRemove(bookId, out Book? book);
        return Task.FromResult(book);
    }

    public Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellation = default)
    {
        return Task.FromResult(_books.Values.AsEnumerable());
    }

    public Task<Book?> GetBookByIdAsync(int bookId, CancellationToken cancellation = default)
    {
        _books.TryGetValue(bookId, out var book);

        return Task.FromResult(book);
    }

    public Task<bool> UpdateBookAsync(Book book, CancellationToken cancellation = default)
    {
        if (!_books.ContainsKey(book.BookId)) 
            return Task.FromResult(false);

        _books[book.BookId] = book;
        return Task.FromResult(true);
    }
}
