namespace Codebreaker.GameAPIs.Data;

public interface ICodebreakerRepository
{
    Task AddGameAsync(Game game, CancellationToken cancellationToken = default);
    Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default);
    Task DeleteGameAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<Game?> GetGameAsync(Guid gameId, bool withTracking = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken cancellationToken = default);
}
