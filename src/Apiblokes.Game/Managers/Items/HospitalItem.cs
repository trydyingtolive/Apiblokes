using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Items;

public class HospitalItem : IUsableItem
{
    private const int X = Constants.XMinimum;
    private const int Y = Constants.YMinimum;

    private readonly PlayerManager playerManager;
    private readonly string? predicate;
    private BlokeManager? blokeManager;

    public HospitalItem( PlayerManager playerManager, string? predicate )
    {
        this.playerManager = playerManager;
        this.predicate = predicate;
    }

    public async Task<string> UseItemAsync()
    {
        await EnsureBlokeAsync();

        if ( blokeManager == null )
        {
            return "Could not locate Apibloke to heal";
        }

        var hp = await blokeManager.RestoreHealthAsync();
        return $"{blokeManager.Name} health has been restored to {hp}HP";
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

        var playerBlokes = await playerManager.GetPersonalBlokes();
        blokeManager = playerBlokes.FirstOrDefault( b => b.Name.ToLower().Contains( predicate ) );
    }
}
