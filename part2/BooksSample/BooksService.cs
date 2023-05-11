using System.Collections.Concurrent;

namespace BooksSample;

public class BooksService
{
    private readonly ConcurrentDictionary<int, Book> _books;

    public BooksService()
    {
        var books = 
            Enumerable.Range(0, 100)
                .Select(i => 
                    new KeyValuePair<int, Book>(i, new Book(i, $"title {i}", "pub")));
        _books = new ConcurrentDictionary<int, Book>(books);
    }

    public Book? GetBookById(int id)
    {
        _books.TryGetValue(id, out var book);
        return book;
    }

    public IEnumerable<Book> GetAllBooks() => _books.Values;

    public void AddBook(Book book)
    {
        _books.TryAdd(book.BookId, book);
    }
}
