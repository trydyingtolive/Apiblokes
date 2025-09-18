using Apiblokes.Game.Data;
using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Model;

namespace Apiblokes.Game.Managers.Players;

public class PlayerManager
{
    private readonly Player player;
    private readonly IDataContext dataContext;
    private readonly IBlokeManagerBuilder blokeManagerBuilder;

    public string PassKey { get => player.PassKey; }
    public string? Name { get => player.Name; }

    public PlayerManager( Player player, IDataContext dataContext, IBlokeManagerBuilder blokeManagerBuilder )
    {
        this.player = player;
        this.dataContext = dataContext;
        this.blokeManagerBuilder = blokeManagerBuilder;
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

    public async Task<string> GetStatusAsync()
    {
        if ( player == null )
        {
            return "You must first create a player.";
        }

        var blokes = await blokeManagerBuilder.AllFromPlayerInventory( player.Id );

        return $"\r\nYou are in a vast field. You see wild Apiblokes scattering before you. \r\nLocation: {player.X}:{player.Y} You have {blokes.Count} blokes.";
    }
}
