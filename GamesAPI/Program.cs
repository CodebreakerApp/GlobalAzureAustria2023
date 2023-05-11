using System.Text.Json.Serialization;

using Codebreaker.GameAPIs;
using Codebreaker.GameAPIs.Data;
using Codebreaker.GameAPIs.Exceptions;
using Codebreaker.GameAPIs.Services;

using GamesAPI.Data;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IGamesService, GamesService>();
builder.Services.AddSingleton<ICodebreakerRepository, InMemoryGamesRepository>();
builder.Services.AddMemoryCache();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Create game
app.MapPost("/games", async (CreateGameRequest request, IGamesService gameService, CancellationToken cancellationToken) =>
{
    Game game;
    try
    {
        game = await gameService.StartGameAsync(request.GameType, request.PlayerName, cancellationToken);
    }
    catch (GameTypeNotFoundException)
    {
        app.Logger.GameTypeNotFound(request.GameType.ToString());
        return Results.BadRequest("Gametype does not exist");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, ex.Message);
        throw;
    }

    return Results.Created($"/games/{game.GameId}", game.ToCreateGameResponse());
})
.WithName("CreateGame")
.WithSummary("Creates and starts a game");

// Update the game resource with a move
app.MapPut("/games/{gameId:guid}/moves", async (Guid gameId, SetMoveRequest request, IGamesService gameService, CancellationToken cancellationToken) =>
{
    try
    {
        var move = request.ToMove();

        Game game = await gameService.SetMoveAsync(gameId, move, cancellationToken);

        return Results.Ok(game.ToSetMoveResponse());
    }
    catch (GameNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithName("SetMove")
.WithSummary("Sets a move for the game with the given id");

// Get games
app.MapGet("/games", async (IGamesService gameService) =>
{
    var games = await gameService.GetAllGamesAsync();

    return Results.Ok(games);
})
.WithName("GetGames")
.WithSummary("Gets all the games");

// Get game by id
app.MapGet("/{gameId:guid}", async (Guid gameId, IGamesService gameService) =>
{
    Game? game = await gameService.GetGameAsync(gameId);

    if (game is null)
        return Results.NotFound();

    return Results.Ok(game);
})
.WithName("GetGame")
.WithSummary("Gets a game by the given id");

app.Run();
