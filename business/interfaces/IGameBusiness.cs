using chessAPI.models.game;

namespace chessAPI.business.interfaces;

public interface IGameBusiness
{
    Task<clsGame?> getGame(long id);
    Task startGame(clsNewGame newGame);
    Task<bool> swapTurn(long id);
}
