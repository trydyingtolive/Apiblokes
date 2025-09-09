using Apiblokes.Game.Data;
using Apiblokes.Game.Model;

namespace Apiblokes.Game.Managers;

public class PlayerManager
{
    private DataContext dataContext;
    private Player player;

    public static async Task< string> CreateNewPlayerAsync()
    {
        var player = new Player();
        var dataContext = new DataContext();
        dataContext.Players.Add( player );
        await dataContext.SaveChangesAsync();
        return player.Id.ToString();
    }

    public PlayerManager( string playerId )
    {
        if ( !Guid.TryParse( playerId, out var id ) )
        {
            throw new Exception( "Player id is not valid" );
        }

        dataContext = new DataContext();

        player = dataContext.Players.FirstOrDefault( p => p.Id == id )
            ?? throw new Exception( "Player could not be found." );
    }

    public async Task<PlayerManager> MovePlayerAsync( string direction )
    {
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

        await dataContext.SaveChangesAsync();

        return this;
    }

    public string GetStatus()
    {
        return $"You are in a vast field. You see wild Apiblokes scattering before you. Location: {player.X}:{player.Y}";
    }
}
