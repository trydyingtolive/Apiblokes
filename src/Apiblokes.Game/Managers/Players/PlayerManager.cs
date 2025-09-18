using System.ComponentModel.Design;
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
        var localBlokes = await blokeManagerBuilder.AllFromWorldLocation( player.X, player.Y );

        var response = $"\r\nYou are in a vast field.  \r\nLocation: {player.X}:{player.Y}";

        if ( localBlokes.Any() )
        {
            response += "\r\n\r\n  You can see:";
            foreach ( var bloke in localBlokes )
            {
                response += $"\r\n    {bloke.Name}: {bloke.Type} Type";
            }
        }

        return response;
    }

    public async Task<string> GetInventoryAsync()
    {
        var blokes = await blokeManagerBuilder.AllFromPlayerInventory( player.Id );

        var response =  $"\r\nInventory:";

        if ( blokes.Any() )
        {
            response += "\r\n  Blokes:";
            foreach ( var bloke in blokes )
            {
                response += $"\r\n    {bloke.Name}: {bloke.Type} Type ({bloke.Health}HP)";
            }
        }
        else
        {
            response += "You have no blokes. You cannot continue.";
        }
        
        return response;
    }
}
