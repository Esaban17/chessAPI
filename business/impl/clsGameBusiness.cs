using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.game;

namespace chessAPI.business.impl;

public sealed class clsGameBusiness<TI, TC> : IGameBusiness<TI>
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IGameRepository<TI, TC> GameRepository;

    public clsGameBusiness(IGameRepository<TI, TC> GameRepository)
    {
        this.GameRepository = GameRepository;
    }

    public async Task<clsGame<TI>> addGame(clsNewGame newGame)
    {
        var x = await GameRepository.addGame(newGame).ConfigureAwait(false);
        return new clsGame<TI>(x, newGame.started, newGame.turn, newGame.winner, newGame.whites, newGame.blacks);
    }
    public async Task<clsGame<TI>> getGame(TI Gameid)
    {
        var Game = await GameRepository.getGame(Gameid).ConfigureAwait(false);
        return new clsGame<TI>(Game.id, Game.started, Game.turn, Game.winner, Game.whites, Game.blacks);
    }

    public async Task<clsGame<TI>> updateGame(clsGame<TI> updatedGame)
    {
        await GameRepository.updateGame(updatedGame).ConfigureAwait(false);
        return updatedGame;
    }
}