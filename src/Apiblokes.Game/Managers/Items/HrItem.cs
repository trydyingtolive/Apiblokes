using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Items;

public class HrItem : IUsableItem
{
    private const int X = Constants.XMaximum;
    private const int Y = Constants.YMaximum;

    private readonly PlayerManager playerManager;
    private readonly string? predicate;
    private BlokeManager? blokeManager;

    public HrItem( PlayerManager playerManager, string? predicate )
    {
        this.playerManager = playerManager;
        this.predicate = predicate;
    }
    public async Task<string[]> UseItemAsync()
    {
        await EnsureBlokeAsync();

        if ( blokeManager == null )
        {
            return ["Could not locate Apibloke to send to HR"];
        }

        var money = await blokeManager.FireBlokeAsync();
        await playerManager.AddMoneyAsync( money );
        return [$"{blokeManager.Name} enters HR and the door shuts behind them. ",
            $"After a minute you hear faint crying.",
            $"You have been paid {money} Apibucks for your betrayal."
        ];


    }

    private async Task EnsureBlokeAsync()
    {
        if ( blokeManager != null )
        {
            return;
        }

        if ( string.IsNullOrEmpty( predicate ) )
        {
            return;
        }

        if ( playerManager.X != X || playerManager.Y != Y )
        {
            return;
        }

        var playerBlokes = await playerManager.GetPersonalBlokesAsync();
        blokeManager = playerBlokes.FirstOrDefault( b => b.Name.ToLower().Contains( predicate ) );
    }
}
