using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.game;
using chessAPI.business.impl;

namespace chessAPI.business.impl
{
    public sealed class clsGameBusiness<TI, TC> : IGameBusiness<TI>
        where TI : struct, IEquatable<TI>
        where TC : struct
    {
        internal readonly IGameRepository<TI, TC> GameRepository;
        internal readonly ITeamRepository<TI, TC> TeamRepository;

        public clsGameBusiness(IGameRepository<TI, TC> GameRepository, ITeamRepository<TI, TC> TeamRepository)
        {
            this.GameRepository = GameRepository;
            this.TeamRepository = TeamRepository;
        }

        public async Task<clsGame<TI>> addGame(clsNewGame newGame)
        {
            var x = await GameRepository.addGame(newGame).ConfigureAwait(false);
            return new clsGame<TI>(x, newGame.started, newGame.turn, newGame.winner, newGame.whites, newGame.blacks);
        }

        public async Task<clsGame<TI>?> getGame(TI GameId)
        {
            var Game = await GameRepository.getGame(GameId).ConfigureAwait(false);
            return new clsGame<TI>(Game.id, Game.started, Game.turn, Game.winner, Game.whites, Game.blacks);
        }

        public async Task<clsGame<TI>> updateGame(clsGame<TI> updatedGame)
        {
            await GameRepository.updateGame(updatedGame).ConfigureAwait(false);
            return updatedGame;
        }
    }
}
