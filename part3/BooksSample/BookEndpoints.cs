using Microsoft.AspNetCore.Http.HttpResults;

namespace BooksSample;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/books")
            .WithTags("Books");

        group.MapGet("/", async (IBooksRepository booksService) =>
        {
            return TypedResults.Ok(await booksService.GetAllBooksAsync());
        })
        .WithName("GetAllBooks");

        group.MapGet("/{bookid}", async Task<Results<Ok<Book>, NotFound>> (int bookId, IBooksRepository booksService) =>
        {
            return await booksService.GetBookByIdAsync(bookId)
                is Book model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBookById");

        group.MapPut("/{bookid}", async Task<Results<NotFound, NoContent>> (int bookId, Book book, IBooksRepository booksService) =>
        {
            return await booksService.UpdateBookAsync(book) ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("UpdateBook");

        group.MapPost("/", async (Book book, IBooksRepository booksService) =>
        {
            book = await booksService.AddBookAsync(book);
            return TypedResults.Created($"/Books/{book.BookId}", book);
        })
        .WithName("CreateBook");

        group.MapDelete("/{bookId}", async Task<Results<NotFound, Ok<Book>>> (int bookId, IBooksRepository booksService) =>
        {
            var book = await booksService.DeleteBookAsync(bookId);
            
            return book is not null ? TypedResults.Ok(book) : TypedResults.NotFound();
        })
        .WithName("DeleteBook");
    }
}
