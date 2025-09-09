using Apiblokes.Game.Data;
using Apiblokes.Game.Managers;

var builder = WebApplication.CreateBuilder( args );
var app = builder.Build();

app.MapGet( "/", ( HttpContext context ) =>
{
    var auth = context.Request.Headers.Authorization.FirstOrDefault();

    if ( string.IsNullOrEmpty( auth ) )
    {
        context.Response.StatusCode = 400;
        return "Please post to this URL to get a player Id. Then put the Id in the Authorization header";
    }

    try
    {
        var manager = new PlayerManager( new DataContext(), auth.Replace( "Bearer ", "" ) );
        return manager.GetStatus();
    }
    catch
    {
        context.Response.StatusCode = 404;
        return "Player not found";
    }
} );

app.MapPost( "/", async () =>
{
    return await PlayerManager.CreateNewPlayerAsync( new DataContext() );
} );


app.MapPost( "/move/{direction}", async ( HttpContext context, string direction ) =>
{
    var auth = context.Request.Headers.Authorization.FirstOrDefault();

    if ( string.IsNullOrEmpty( auth ) )
    {
        context.Response.StatusCode = 400;
        return "Please post to this URL to get a player Id. Then put the Id in the Authorization header";
    }

    try
    {
        var manager = new PlayerManager( new DataContext(), auth.Replace( "Bearer ", "" ) );
        return ( await manager.MovePlayerAsync( direction ) )
        .GetStatus();
    }
    catch
    {
        context.Response.StatusCode = 404;
        return "Player not found";
    }
} );

app.Run();
