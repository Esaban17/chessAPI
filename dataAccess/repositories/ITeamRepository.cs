using chessAPI.dataAccess.models;
using chessAPI.models.game;
using chessAPI.models.team;

namespace chessAPI.dataAccess.repositores;

public interface ITeamRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    Task<TI> addTeam(clsNewTeam Team);
    Task<clsTeamEntityModel<TI, TC>> getTeam(TI id);

    Task<IEnumerable<clsTeamEntityModel<TI, TC>>> addTeams(IEnumerable<clsNewTeam> Teams);
    Task<IEnumerable<clsTeamEntityModel<TI, TC>>> getTeamsByGame(TI gameId);
    Task updateTeam(clsTeam<TI> updatedTeam);
    Task deleteTeam(TI id);
}