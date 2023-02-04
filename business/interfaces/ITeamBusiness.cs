using chessAPI.models.game;
using chessAPI.models.team;

namespace chessAPI.business.interfaces;

public interface ITeamBusiness<TI>
    where TI : struct, IEquatable<TI>
{
    Task<clsTeam<TI>> addTeam(clsNewTeam newTeam);
    Task<clsTeam<TI>> getTeam(TI teamId);
    Task<clsTeam<TI>> updateTeam(clsTeam<TI> updatedTeam);
}