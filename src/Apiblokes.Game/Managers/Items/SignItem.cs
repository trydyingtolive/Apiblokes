using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Items
{
    public class SignItem : IUsableItem
    {
        PlayerManager playerManager;

        public SignItem( PlayerManager playerManager )
        {
            this.playerManager = playerManager;
        }

        public async Task<string[]> UseItemAsync()
        {
            //Start location sign
            if (playerManager.X == Constants.XStart && playerManager.Y == Constants.YStart )
            {
                return ["Welcome to Apiblokes.", 
                    "If you need help type 'h' for all available commands",
                    "",
                    "Throughout this world you will find Apiblokes who you can battle and capture",
                    "Managers are common and can be captured without needing anything special.",
                    $"Network, System, Developer, and Help Desk Apiblokes will require a {Constants.Level2CatcherName}",
                    $"Do It All Apiblokes will require a {Constants.Level3CatcherName}",
                    "",
                    "At each corner of the map you will find vending machines for buying different items, coffee makers for healing your Apiblokes, and HR departments for trading your Apiblokes for Apibucks",
                    "",
                    "Source code at github.com/trydyingtolive/apiblokes"];
            }

            //Timely joke
            if ( playerManager.X == 6 && playerManager.Y == 7 )
            {
                return ["Siiix Seeeeeven", "--Random Middle Schooler"];
            }

            return ["There is nothing here to read"];
        }
    }
}