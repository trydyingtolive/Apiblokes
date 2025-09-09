using Apiblokes.Game.Data;
using Apiblokes.Game.Managers;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddScoped<IDataContext, DataContext>();

var app = builder.Build();

app.MapGet( "/", ( HttpContext context, IDataContext dataContext ) =>
{
    var auth = context.Request.Headers.Authorization.FirstOrDefault();

    if ( string.IsNullOrEmpty( auth ) )
    {
        context.Response.StatusCode = 400;
        return "Please post to this URL to get a player Id. Then put the Id in the Authorization header";
    }

    try
    {
        var manager = new PlayerManager( dataContext, auth.Replace( "Bearer ", "" ) );
        return manager.GetStatus();
    }
    catch
    {
        context.Response.StatusCode = 404;
        return "Player not found";
    }
} );

app.MapPost( "/", async (IDataContext dataContext) =>
{
    return await PlayerManager.CreateNewPlayerAsync( dataContext );
} );


app.MapPost( "/move/{direction}", async ( HttpContext context, string direction, IDataContext dataContext ) =>
{
    var auth = context.Request.Headers.Authorization.FirstOrDefault();

    if ( string.IsNullOrEmpty( auth ) )
    {
        context.Response.StatusCode = 400;
        return "Please post to this URL to get a player Id. Then put the Id in the Authorization header";
    }

    try
    {
        var manager = new PlayerManager( dataContext, auth.Replace( "Bearer ", "" ) );
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
