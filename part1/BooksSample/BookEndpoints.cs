using Microsoft.EntityFrameworkCore;
namespace BooksSample;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Book", async (BooksContext db) =>
        {
            return await db.Books.ToListAsync();
        })
        .WithName("GetAllBooks")
        .WithTags("Books")
        .Produces<List<Book>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Book/{id}", async (int BookId, BooksContext db) =>
        {
            return await db.Books.FindAsync(BookId)
                is Book model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetBookById")
        .WithTags("Books")
        .Produces<Book>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Book/{id}", async (int BookId, Book book, BooksContext db) =>
        {
            var foundModel = await db.Books.FindAsync(BookId);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(book);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateBook")
        .WithTags("Books")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Book/", async (Book book, BooksContext db) =>
        {
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return Results.Created($"/Books/{book.BookId}", book);
        })
        .WithName("CreateBook")
        .WithTags("Books")
        .Produces<Book>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Book/{id}", async (int BookId, BooksContext db) =>
        {
            if (await db.Books.FindAsync(BookId) is Book book)
            {
                db.Books.Remove(book);
                await db.SaveChangesAsync();
                return Results.Ok(book);
            }

            return Results.NotFound();
        })
        .WithName("DeleteBook")
        .WithTags("Books")
        .Produces<Book>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
