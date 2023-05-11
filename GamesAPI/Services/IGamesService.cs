namespace Codebreaker.GameAPIs.Services;

public interface IGamesService
{
    ValueTask<Game?> GetGameAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken cancellationToken = default);

    Task<Game> StartGameAsync(GameType gameType, string playerName, CancellationToken cancellationToken = default);

    Task DeleteGameAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Game> SetMoveAsync(Guid gameId, Move move, CancellationToken cancellationToken = default);
}
