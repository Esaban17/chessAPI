using Autofac;
using Autofac.Extensions.DependencyInjection;
using chessAPI;
using chessAPI.business.interfaces;
using chessAPI.models.game;
using chessAPI.models.player;
using chessAPI.models.team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;

//Serilog logger (https://github.com/serilog/serilog-aspnetcore)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("chessAPI starting");
    var builder = WebApplication.CreateBuilder(args);

    var connectionStrings = new connectionStrings();
    builder.Services.AddOptions();
    builder.Services.Configure<connectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

    // Two-stage initialization (https://github.com/serilog/serilog-aspnetcore)
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom
             .Configuration(context.Configuration)
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning).ReadFrom
             .Services(services).Enrich
             .FromLogContext().WriteTo
             .Console());

    // Autofac como inyección de dependencias
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new chessAPI.dependencyInjection<int, int>()));
    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseMiddleware(typeof(chessAPI.customMiddleware<int>));
    app.MapGet("/", () =>
    {
        return "hola mundo";
    });

    //INSERTAR UN JUGADOR
    app.MapPost("player", 
    [AllowAnonymous] async(IPlayerBusiness<int> bs, clsNewPlayer newPlayer) => Results.Ok(await bs.addPlayer(newPlayer)));

    //OBTENER UN JUGADOR
    app.MapGet("player/{idPlayer}",
    [AllowAnonymous] async (IPlayerBusiness<int> bs, int idPlayer) => Results.Ok(await bs.getPlayer(idPlayer)));

    //MODIFICAR UN JUGADOR
    app.MapPut("player/{idPlayer}",
    [AllowAnonymous] async (IPlayerBusiness<int> bs, int idPlayer, clsPlayer<int> updatePlayer) => Results.Ok(await bs.updatePlayer(updatePlayer)));

    //INSERTAR UN EQUIPO
    app.MapPost("team",
    [AllowAnonymous] async (ITeamBusiness<int> bs, clsNewTeam newTeam) => Results.Ok(await bs.addTeam(newTeam)));

    //OBTENER UN EQUIPO
    app.MapGet("team/{idTeam}",
    [AllowAnonymous] async (ITeamBusiness<int> bs, int idTeam) => Results.Ok(await bs.getTeam(idTeam)));

    //MODIFICAR UN EQUIPO
    app.MapPut("team/{idTeam}",
    [AllowAnonymous] async (ITeamBusiness<int> bs, int idTeam, clsTeam<int> updateTeam) => Results.Ok(await bs.updateTeam(updateTeam)));


    //OBTENER UN JUEGO POR ID
    app.MapGet("game/{idGame}",
    [AllowAnonymous] async (IGameBusiness<int> bs, int idGame) => Results.Ok(await bs.getGame(idGame)));

    //INICIAR UN JUEGO
    app.MapPost("game",
    [AllowAnonymous] async (IGameBusiness<int> bs, ITeamBusiness<int> bsTeam, clsNewGame newGame) => 
    {
        var teamWhites = await bsTeam.getTeam(newGame.whites);

        if (teamWhites != null)
        {
            Results.Ok(await bs.addGame(newGame));
        }
        else
        {
            throw new System.NullReferenceException();
        }
    });

    //UNIRSE A UN JUEGO
    app.MapPut("game/{idGame}",
    [AllowAnonymous] async (IGameBusiness<int> bs, int idGame, ITeamBusiness<int> bsTeam, clsGame<int> updateGame) => {

        var teamBlacks = await bsTeam.getTeam(updateGame.blacks);

        if (teamBlacks != null)
        {
            Results.Ok(await bs.updateGame(updateGame));
        }
        else
        {
            throw new System.NullReferenceException();
        }
        
     });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "chessAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
