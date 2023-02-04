using Autofac;
using Npgsql;
using Microsoft.Extensions.Options;
using System.Data;
using chessAPI.dataAccess.providers.postgreSQL;
using chessAPI.dataAccess.interfaces;
using chessAPI.dataAccess.common;
using chessAPI.dataAccess.queries.postgreSQL;
using chessAPI.dataAccess.queries;
using chessAPI.dataAccess.repositores;
using chessAPI.business.interfaces;
using chessAPI.business.impl;

namespace chessAPI;

public sealed class dependencyInjection<TC, TI> : Module
where TI : struct, IEquatable<TI>
        where TC : struct
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.Register(c => new NpgsqlConnection(c.Resolve<IOptions<connectionStrings>>().Value.relationalDBConn))
            .InstancePerLifetimeScope()
            .As<IDbConnection>();

        #region "Low level DAL Infrastructure"
        builder.Register(c => new clsConcurrency<TC>())
            .SingleInstance()
            .As<IDBConcurrencyHandler<TC>>();
        builder.Register(c => new clsRelationalContext<TC>(c.Resolve<IDbConnection>(),
                                                           c.Resolve<ILogger<clsRelationalContext<TC>>>(),
                                                           c.Resolve<IDBConcurrencyHandler<TC>>()))
                .InstancePerLifetimeScope()
                .As<IRelationalContext<TC>>();
        #endregion

        #region "Queries"
        builder.Register(c => new qPlayer())
          .SingleInstance()
          .As<IQPlayer>();


        builder.Register(c => new qGame())
          .SingleInstance()
          .As<IQGame>();

        builder.Register(c => new qTeam())
            .SingleInstance()
            .As<IQTeam>();
        #endregion

        #region "Repositories"
        builder.Register(c => new clsPlayerRepository<TI, TC>(c.Resolve<IRelationalContext<TC>>(),
                                                              c.Resolve<IQPlayer>(),
                                                              c.Resolve<ILogger<clsPlayerRepository<TI, TC>>>()))
               .InstancePerDependency()
               .As<IPlayerRepository<TI, TC>>();


        builder.Register(c => new clsGameRepository<TI, TC>(c.Resolve<IRelationalContext<TC>>(),
                                                             c.Resolve<IQGame>(),
                                                             c.Resolve<ILogger<clsGameRepository<TI, TC>>>()))
              .InstancePerDependency()
              .As<IGameRepository<TI, TC>>();


        builder.Register(c => new clsTeamRepository<TI, TC>(c.Resolve<IRelationalContext<TC>>(),
                                                     c.Resolve<IQTeam>(),
                                                     c.Resolve<ILogger<clsTeamRepository<TI, TC>>>()))
               .InstancePerDependency()
               .As<ITeamRepository<TI, TC>>();
        #endregion

        #region "Kaizen Entity Factories"
        builder.Register<Func<IPlayerRepository<TI, TC>>>(delegate (IComponentContext context)
        {
            IComponentContext cc = context.Resolve<IComponentContext>();
            return cc.Resolve<IPlayerRepository<TI, TC>>;
        });

        builder.Register<Func<IGameRepository<TI, TC>>>(delegate (IComponentContext context)
        {
            IComponentContext cc = context.Resolve<IComponentContext>();
            return cc.Resolve<IGameRepository<TI, TC>>;
        });

        builder.Register<Func<ITeamRepository<TI, TC>>>(delegate (IComponentContext context)
        {
            IComponentContext cc = context.Resolve<IComponentContext>();
            return cc.Resolve<ITeamRepository<TI, TC>>;
        });
        #endregion

        #region "Business classes"
        builder.Register(c => new clsPlayerBusiness<TI, TC>(c.Resolve<IPlayerRepository<TI, TC>>()))
               .InstancePerDependency()
               .As<IPlayerBusiness<TI>>();

        builder.Register(c => new clsGameBusiness<TI, TC>(c.Resolve<IGameRepository<TI, TC>>()))
               .InstancePerDependency()
               .As<IGameBusiness<TI>>();

        builder.Register(c => new clsTeamBusiness<TI, TC>(c.Resolve<ITeamRepository<TI, TC>>()))
               .InstancePerDependency()
               .As<ITeamBusiness<TI>>();
        #endregion
    }
}