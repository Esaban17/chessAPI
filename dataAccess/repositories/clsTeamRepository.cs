using chessAPI.dataAccess.common;
using chessAPI.dataAccess.interfaces;
using chessAPI.dataAccess.models;
using chessAPI.models.game;
using chessAPI.models.player;
using chessAPI.models.team;
using Dapper;

namespace chessAPI.dataAccess.repositores;

public sealed class clsTeamRepository<TI, TC> : clsDataAccess<clsTeamEntityModel<TI, TC>, TI, TC>, ITeamRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    public clsTeamRepository(IRelationalContext<TC> rkm,
                               ISQLData queries,
                               ILogger<clsTeamRepository<TI, TC>> logger) : base(rkm, queries, logger)
    {
    }

    public async Task<TI> addTeam(clsNewTeam team)
    {
        var p = new DynamicParameters();
        p.Add("NAME", team.name);
        return await add<TI>(p).ConfigureAwait(false);
    }

    public async Task<clsTeamEntityModel<TI, TC>> getTeam(TI teamId)
    {
        return await getEntity(teamId).ConfigureAwait(false);
    }

    public async Task<IEnumerable<clsTeamEntityModel<TI, TC>>> addTeams(IEnumerable<clsNewTeam> teams)
    {
        var r = new List<clsTeamEntityModel<TI, TC>>(teams.Count());
        foreach (var team in teams)
        {
            TI teamId = await addTeam(team).ConfigureAwait(false);
            r.Add(new clsTeamEntityModel<TI, TC>() { id = teamId, name = team.name });
        }
        return r;
    }

    public Task deleteTeam(TI id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<clsTeamEntityModel<TI, TC>>> getTeamsByGame(TI gameId)
    {
        throw new NotImplementedException();
    }

    public async Task updateTeam(clsTeam<TI> updatedTeam)
    {
        var p = fieldsAsParams(new clsTeamEntityModel<TI, TC>() { id = updatedTeam.id, name = updatedTeam.name });
        await set(p, null).ConfigureAwait(false);
    }

    protected override DynamicParameters fieldsAsParams(clsTeamEntityModel<TI, TC> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var p = new DynamicParameters();
        p.Add("ID", entity.id);
        p.Add("NAME", entity.name);
        return p;
    }

    protected override DynamicParameters keyAsParams(TI key)
    {
        var p = new DynamicParameters();
        p.Add("ID", key);
        return p;
    }
}