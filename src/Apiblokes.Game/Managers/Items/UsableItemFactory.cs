using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Items;

public class UsableItemFactory : IUsableItemFactory
{
    public IUsableItem? GetUsableItem( PlayerManager playerManager, string requestText )
    {
        var parts = requestText.ToLower().Split( "on" );
        string? subject = null;
        string? predicate = null;

        if ( parts.Length > 0 )
        {
            subject = parts[0].Trim();
        }
        else
        {
            return null;
        }

        if ( parts.Length > 1 )
        {
            predicate = parts[1].Trim();
        }

        switch ( subject )
        {
            case "hospital":
                return new HospitalItem( playerManager, predicate );
            case "hr":
                return new HrItem( playerManager, predicate );
            default:
                break;
        }


        return null;
    }
}
