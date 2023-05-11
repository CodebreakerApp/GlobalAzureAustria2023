using System.Collections.Concurrent;

using Codebreaker.GameAPIs.Data;

namespace GamesAPI.Data;

public class InMemoryGamesRepository : ICodebreakerRepository
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();

    public Task AddGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        _games.TryAdd(game.GameId, game);
        return Task.CompletedTask;
    }

    public Task DeleteGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        _games.Remove(gameId, out Game? _);
        return Task.CompletedTask;
    }

    public Task<Game?> GetGameAsync(Guid gameId, bool withTracking = true, CancellationToken cancellationToken = default)
    {
        _games.TryGetValue(gameId, out Game? game);
        return Task.FromResult(game);
    }

    public Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_games.Values.ToList().AsEnumerable());
    }

    public Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        _games.AddOrUpdate(game.GameId, game, (id, existingGame) => game);

        return Task.CompletedTask;
    }
}
