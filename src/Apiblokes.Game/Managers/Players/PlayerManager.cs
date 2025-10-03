using Apiblokes.Game.Data;
using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Battle;
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
    public Guid Id { get => player.Id; }
    public int X { get => player.X; }
    public int Y { get => player.Y; }
    public int Money { get => player.Money; }

    public PlayerManager( Player player, IDataContext dataContext, IBlokeManagerBuilder blokeManagerBuilder )
    {
        this.player = player;
        this.dataContext = dataContext;
        this.blokeManagerBuilder = blokeManagerBuilder;
    }

    public event EventHandler OnGlobalNotification;

    protected virtual void NotifyGlobally( GlobalNotificationArgs e )
    {
        OnGlobalNotification?.Invoke( this, e );
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
        var localBlokes = await blokeManagerBuilder.AllFromWorldLocationAsync( player.X, player.Y );

        var response = $"\r\n{string.Join( "\r\n", LocationFlavor.GetLocationFlavor( player.X, player.Y ) )}  \r\nLocation: {player.X}:{player.Y}";

        if ( localBlokes.Any() )
        {
            response += "\r\n\r\n  You can see:";
            foreach ( var bloke in localBlokes )
            {
                response += $"\r\n    {bloke.Name}: {bloke.Type} Type";
            }
        }

        var signText = SignTextHelper.GetSignText(player.X, player.Y);
        if (signText != null )
        {
            response += $"\r\n\r\nThere is a sign here. (type 'read sign' to read it)";
        }

        return response;
    }

    public async Task<string> GetInventoryAsync()
    {
        var blokes = await GetPersonalBlokesAsync();

        var response = $"\r\nInventory:";

        response += $"\r\n  Apibucks: {player.Money} ";

        response += $"\r\n  Cubicles: {player.Level2Catchers}";
        response += $"\r\n  Offices: {player.Level3Catchers}";

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
            response += "\r\nYou have no blokes. You cannot continue.";
        }

        return response;
    }

    public async Task<string[]> AttemptAttackAsync( string arguments )
    {
        var output = new List<string>();
        var options = new BattleRequestOptions
        {
            AvailablePlayerBlokes = await GetPersonalBlokesAsync(),
            RequestText = arguments,
            X = player.X,
            Y = player.Y
        };

        var (battleManager, text) = await BattleManager.SetupBattleAsync( blokeManagerBuilder, options );

        output.Add( text );

        if ( battleManager != null )
        {
            output.AddRange( await battleManager.ProcessBattleAsync() );
        }

        return output.ToArray();
    }

    public async Task<List<BlokeManager>> GetPersonalBlokesAsync()
    {
        return await blokeManagerBuilder.AllFromPlayerInventory( player.Id );
    }

    public async Task AddMoneyAsync( int money )
    {
        player.Money += money;
        await dataContext.SaveChangesAsync();
    }

    public async Task AddCatcherAsync( int catcherLevel, int quantity )
    {
        if ( catcherLevel == 2 )
        {
            player.Level2Catchers += quantity;
            player.Level2Catchers = Math.Max( 0, player.Level2Catchers );
            await dataContext.SaveChangesAsync();
        }

        if ( catcherLevel == 3 )
        {
            player.Level3Catchers += quantity;
            player.Level3Catchers = Math.Max( 0, player.Level3Catchers );
            await dataContext.SaveChangesAsync();
        }
    }

    public async Task<string[]> AttemptCaptureAsync( string arguments )
    {
        BlokeManager? blokeManager = null;
        var blokeManagers = await blokeManagerBuilder
            .AllFromWorldLocationAsync( player.X, player.Y );

        if ( blokeManagers.Count == 0 )
        {
            return ["There are no blokes at your current location."];
        }
        else if ( blokeManagers.Count == 1 )
        {
            blokeManager = blokeManagers[0];
        }
        else
        {
            blokeManager = blokeManagers
               .FirstOrDefault( b => b.Name.ToLower().Contains( arguments.ToLower().Trim() ) );
        }

        if ( blokeManager == null )
        {
            return [$"Could not find an Apibloke named: {arguments.Trim()}"];
        }

        if ( blokeManager.Health != 0 )
        {
            return [$"{blokeManager.Name} laughs at your pitiful attempt. (Apiblokes must be at 0 health to capture.)"];
        }

        if ( blokeManager.CaptureLevel == 2 )
        {
            if ( player.Level2Catchers >= 1 )
            {
                await AddCatcherAsync( 2, -1 );
            }
            else if ( player.Level3Catchers >= 1 )
            {
                await AddCatcherAsync( 3, -1 );
            }
            else
            {
                return [$"{blokeManager.Name} requires a minimum of a {Constants.Level2CatcherName} to capture."];
            }
        }

        if ( blokeManager.CaptureLevel == 3 )
        {
            if ( player.Level3Catchers >= 1 )
            {
                await AddCatcherAsync( 3, -1 );
            }
            else
            {
                return [$"{blokeManager.Name} requires a {Constants.Level3CatcherName} to capture."];
            }
        }

        await blokeManager.MoveToPlayer( player.Id );

        //Notify everyone that we captured a non-common
        if ( blokeManager.CaptureLevel > 1 )
        {
            NotifyGlobally( new GlobalNotificationArgs
            {
                Message = $"{player.Name} captured a {blokeManager.Type}"
            } );
        }

        return [$"{blokeManager.Name}, thankful to be employed, joins your team."];
    }
}
