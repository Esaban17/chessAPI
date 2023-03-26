using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.game;
using chessAPI.business.impl;

namespace chessAPI.business.impl
{
    public sealed class clsGameBusiness : IGameBusiness
    {
        internal readonly IGameRepository gameRepository;

        public clsGameBusiness(IGameRepository gameRepository) => this.gameRepository = gameRepository;

        public async Task startGame(clsNewGame newGame) => await gameRepository.addGame(newGame).ConfigureAwait(false);

        public async Task<clsGame?> getGame(long id)
        {
            var x = await gameRepository.getGame(id).ConfigureAwait(false);
            return x != null ? (clsGame)x : null;
        }

        public async Task<bool> swapTurn(long id)
        {
            var x = await gameRepository.getGame(id).ConfigureAwait(false);
            if (x != null)
            {
                await gameRepository.swapTurn(id).ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}
