using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.player;

namespace chessAPI.business.impl;

public sealed class clsPlayerBusiness<TI, TC> : IPlayerBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IPlayerRepository<TI, TC> playerRepository;

    public clsPlayerBusiness(IPlayerRepository<TI, TC> playerRepository)
    {
        this.playerRepository = playerRepository;
    }

    public async Task<clsPlayer<TI>> addPlayer(clsNewPlayer newPlayer)
    {
        var x = await playerRepository.addPlayer(newPlayer).ConfigureAwait(false);
        return new clsPlayer<TI>(x, newPlayer.email);
    }

    public async Task<List<clsPlayer<TI>>> getPlayers(TI gameId)
    {
        List<clsPlayer<TI>> players = (List<clsPlayer<TI>>)await playerRepository.getPlayersByGame(gameId).ConfigureAwait(false);
        return players;
    }

    public Task<clsPlayer<TI>> updatePlayer(clsPlayer<TI> player)
    {
        throw new NotImplementedException();
    }
}