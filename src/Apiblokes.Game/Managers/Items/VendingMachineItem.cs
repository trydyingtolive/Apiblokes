using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Items;

public class VendingMachineItem : IUsableItem
{
    private const int Level2X = Constants.XMinimum;
    private const int Level2Y = Constants.YMaximum;

    private const int Level2Cost = 10;
    private const string Level2Name = Constants.Level2CatcherName;

    private const int Level3X = Constants.XMaximum;
    private const int Level3Y = Constants.YMinimum;

    private const int Level3Cost = 50;
    private const string Level3Name = Constants.Level3CatcherName;

    private readonly PlayerManager playerManager;
    public VendingMachineItem( PlayerManager playerManager )
    {
        this.playerManager = playerManager;
    }

    public async Task<string[]> UseItemAsync()
    {
        int? cost = null;
        string? itemName = null;
        int level = 0;

        if ( playerManager.X == Level2X && playerManager.Y == Level2Y )
        {
            cost = Level2Cost;
            itemName = Level2Name;
            level = 2;
        }

        if ( playerManager.X == Level3X && playerManager.Y == Level3Y )
        {
            cost = Level3Cost;
            itemName = Level3Name;
            level = 3;
        }

        if ( cost == null || string.IsNullOrEmpty( itemName ) )
        {
            return ["There is no vending here."];
        }

        if ( playerManager.Money < cost )
        {
            return [$"You check your pockets for loose change, but you can't come up with {cost} Apibucks."];
        }

        await playerManager.AddMoneyAsync( -cost.Value );
        await playerManager.AddCatcherAsync( level, 1 );

        return ["You put your wrinkled money into the machine.",
            "It whirrs and shutters making awful grinding noises.",
            $"With a clunk, a brand new miniaturized {itemName} drops into the tray. You pick it up and put it in your pocket."
        ];
    }
}
