using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using TaskOkBookNotFound = System.Threading.Tasks.Task<Microsoft.AspNetCore.Http.HttpResults.Results<Microsoft.AspNetCore.Http.HttpResults.Ok<BooksSample.Book>, Microsoft.AspNetCore.Http.HttpResults.NotFound>>;
namespace BooksSample;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/books")
            .WithTags("Books");

        group.MapGet("/", async (BooksContext db) =>
        {
            var books = await db.Books.ToListAsync();
            return TypedResults.Ok(books);
        })
        .WithName("GetAllBooks");

        group.MapGet("/{bookid}", async Task<Results<Ok<Book>, NotFound>> (int bookId, BooksContext db) =>
        {
            return await db.Books.FindAsync(bookId)
                is Book model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBookById");

        group.MapPut("/{bookid}", async Task<Results<NotFound, NoContent>> (int bookId, Book book, BooksContext db) =>
        {
            var foundModel = await db.Books.AsNoTracking().SingleOrDefaultAsync(b => b.BookId == bookId);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            db.Update(book);

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateBook");

        group.MapPost("/", async (Book book, BooksContext db) =>
        {
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/Books/{book.BookId}", book);
        })
        .WithName("CreateBook");

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok<Book>>> (int BookId, BooksContext db) =>
        {
            if (await db.Books.FindAsync(BookId) is Book book)
            {
                db.Books.Remove(book);
                await db.SaveChangesAsync();
                return TypedResults.Ok(book);
            }

            return TypedResults.NotFound();
        })
        .WithName("DeleteBook");
    }
}
