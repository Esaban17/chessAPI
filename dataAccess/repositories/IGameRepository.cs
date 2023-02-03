using chessAPI.dataAccess.models;
using chessAPI.models.game;

namespace chessAPI.dataAccess.repositores;

public interface IGameRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    Task<TI> addGame(clsNewGame Game);
    Task<clsGameEntityModel<TI, TC>> getGame(TI id);

    Task<IEnumerable<clsGameEntityModel<TI, TC>>> addGames(IEnumerable<clsNewGame> Games);
    Task<IEnumerable<clsGameEntityModel<TI, TC>>> getGamesByGame(TI gameId);
    Task updateGame(clsGame<TI> updatedGame);
    Task deleteGame(TI id);
}