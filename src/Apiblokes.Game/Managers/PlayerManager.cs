using Apiblokes.Game.Data;
using Apiblokes.Game.Helpers;
using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Game.Managers;

public class PlayerManager
{
    private readonly IDataContext dataContext;
    private Player? player;

    public PlayerManager( IDataContext dataContext )
    {
        this.dataContext = dataContext;
    }

    public async Task<string> CreateNewPlayerAsync()
    {

        player = new Player();
        dataContext.Players.Add( player );
        await dataContext.SaveChangesAsync();

        var blokeManager = new BlokeManager( dataContext );
        await blokeManager.CreateStarterBlokeAsync( player.Id );

        return player.Id.ToString();

    }

    public async Task<PlayerManager> GetPlayerAsync( string playerId )
    {
        if ( Guid.TryParse( playerId, out var id ) )
        {
            player = await dataContext.Players.FirstOrDefaultAsync( p => p.Id == id );
        }
        return this;
    }

    public async Task MovePlayerAsync( string direction )
    {
        if ( player == null )
        {
            return;
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
                player.X += 1;
                break;
            default:
                break;
        }

        player.X = Math.Clamp( player.X, Constants.XMinimum, Constants.XMaximum );
        player.Y = Math.Clamp( player.Y, Constants.YMinimum, Constants.YMaximum );

        await dataContext.SaveChangesAsync();
    }

    public string GetStatus()
    {
        if ( player == null )
        {
            return "You must first create a player.";
        }

        return $"You are in a vast field. You see wild Apiblokes scattering before you. Location: {player.X}:{player.Y}";
    }
}
