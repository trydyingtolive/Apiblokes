using Apiblokes.Game.Data;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Helpers;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddScoped<IDataContextFactory, DataContextFactory>();
builder.Services.AddScoped<IPlayerManagerBuilder, PlayerManagerBuilder>();
builder.Services.AddScoped<IBlokeManagerBuilder, BlokeManagerBuilder>();

builder.Services.AddHostedService<WorldPopulationManager>();

var app = builder.Build();

app.MapGet( "/", async ( HttpContext context, IPlayerManagerBuilder playerManagerBuilder ) =>
{
    var playerManager = await playerManagerBuilder
        .FromKeyAsync( context.GetPlayerId() );

    if ( playerManager == null )
    {
        return string.Empty;
    }

    return await playerManager.GetStatusAsync();
} );

app.MapPost( "/{name}", async ( HttpContext context, IPlayerManagerBuilder playerManagerBuilder, string name ) =>
{
    var playerManager = await playerManagerBuilder.FromNewPlayer( name );
    return playerManager.PassKey;
} );


app.MapPost( "/move/{direction}", async ( HttpContext context, string direction, IPlayerManagerBuilder playerManagerBuilder ) =>
{
    var playerManager = await playerManagerBuilder.FromKeyAsync( context.GetPlayerId() );

    if ( playerManager == null )
    {
        return "";
    }

    await playerManager.MovePlayerAsync( direction );
    return await playerManager.GetStatusAsync();
} );

app.Run();
