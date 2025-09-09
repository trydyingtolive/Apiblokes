using Apiblokes.Game.Data;
using Apiblokes.Game.Managers;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IGameManager, GameManager>();

var app = builder.Build();

app.MapGet( "/", async ( HttpContext context, IGameManager gameManager ) =>
{
    var auth = context.Request.Headers.Authorization.FirstOrDefault();

    if ( string.IsNullOrEmpty( auth ) )
    {
        context.Response.StatusCode = 400;
        return "Please post to this URL to get a player Id. Then put the Id in the Authorization header";
    }

    var playerManager = await gameManager
        .GetPlayerManagerAsync( auth.Replace( "Bearer ", "" ) );

    return playerManager.GetStatus();
} );

app.MapPost( "/", async ( IGameManager gameManager ) =>
{
    var playerManager = await gameManager.GetPlayerManagerAsync( "" );
    return await playerManager.CreateNewPlayerAsync();
} );


app.MapPost( "/move/{direction}", async ( HttpContext context, string direction, IGameManager gameManager ) =>
{
    var auth = context.Request.Headers.Authorization.FirstOrDefault();

    if ( string.IsNullOrEmpty( auth ) )
    {
        context.Response.StatusCode = 400;
        return "Please post to this URL to get a player Id. Then put the Id in the Authorization header";
    }

    var playerManager = await gameManager.GetPlayerManagerAsync( auth.Replace( "Bearer ", "" ) );
    await playerManager.MovePlayerAsync( direction );
    return playerManager.GetStatus();
} );

app.Run();
