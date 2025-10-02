using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Items
{
    public class SignItem : IUsableItem
    {
        private const string SIGN_MARKER = "============================================================================================";

        PlayerManager playerManager;

        public SignItem( PlayerManager playerManager )
        {
            this.playerManager = playerManager;
        }

        public async Task<string[]> UseItemAsync()
        {
            var text = SignTextHelper.GetSignText( playerManager.X, playerManager.Y );
            if ( text != null )
            {
                var output = new List<string> { SIGN_MARKER };
                output.AddRange( text );
                output.Add( SIGN_MARKER );
                return output.ToArray();
            }


            return ["There is nothing here to read"];
        }
    }
}