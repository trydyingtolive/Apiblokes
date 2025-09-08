using Apiblokes.Data;
using Apiblokes.Model;

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

    if (!Guid.TryParse( auth.Replace( "Bearer ", "" ), out var id ) )
    {
        return $"{auth} is not a valid header";
    }

    var dataContext = new DataContext();
    var player = dataContext.Players.FirstOrDefault( p => p.Id == id );

    if (player == null )
    {
        context.Response.StatusCode = 404;
        return "Player not found";
    }

    return $"You are in a vast field. You see wild Apiblokes scattering before you. Location: {player.X}:{player.Y}";
    } );

app.MapPost( "/", () =>
{
    var player = new Player();
    var context = new DataContext();
    context.Players.Add( player );
    context.SaveChanges();
    return player.Id;
} );


app.MapPost( "/move/{direction}", ( HttpContext context, string direction ) =>
{
    var auth = context.Request.Headers.Authorization.FirstOrDefault();

    if ( string.IsNullOrEmpty( auth ) )
    {
        context.Response.StatusCode = 400;
        return "Please post to this URL to get a player Id. Then put the Id in the Authorization header";
    }

    if ( !Guid.TryParse( auth.Replace( "Bearer ", "" ), out var id ) )
    {
        return $"{auth} is not a valid header";
    }

    var dataContext = new DataContext();
    var player = dataContext.Players.FirstOrDefault( p => p.Id == id );

    if ( player == null )
    {
        context.Response.StatusCode = 404;
        return "Player not found";
    }

    var simpleDir = direction.ToLower().FirstOrDefault();

    switch ( simpleDir )
    {
        case 'n':
            player.Y += 1;
            break;
        case 's':
            player.Y -= 1;
            break;
        case 'w':
            player.X -= 1;
            break;
        case 'e':
            player.Y += 1;
            break;
        default:
            break;
    }

    dataContext.SaveChanges();

    return $"You are in a vast field. You see wild Apiblokes scattering before you. Location: {player.X}:{player.Y}";
} );

app.Run();
