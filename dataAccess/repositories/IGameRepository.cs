using chessAPI.dataAccess.models;
using chessAPI.models.game;

namespace chessAPI.dataAccess.repositores;

public interface IGameRepository
{
    Task addGame(clsNewGame newGame);
    Task<clsGameEntityModel?> getGame(long id);
    Task swapTurn(long id);
}