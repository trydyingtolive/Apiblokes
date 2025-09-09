using Apiblokes.Game.Data;
using Apiblokes.Game.Managers;
using Apiblokes.Helpers;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IGameManager, GameManager>();

var app = builder.Build();

app.MapGet( "/", async ( HttpContext context, IGameManager gameManager ) =>
{
    var playerManager = await gameManager
        .GetPlayerManagerAsync( context.GetPlayerId() );

    return playerManager.GetStatus();
} );

app.MapPost( "/", async ( IGameManager gameManager ) =>
{
    var playerManager = await gameManager.GetPlayerManagerAsync( "" );
    return await playerManager.CreateNewPlayerAsync();
} );


app.MapPost( "/move/{direction}", async ( HttpContext context, string direction, IGameManager gameManager ) =>
{
    var playerManager = await gameManager.GetPlayerManagerAsync( context.GetPlayerId() );
    await playerManager.MovePlayerAsync( direction );
    return playerManager.GetStatus();
} );

app.Run();
