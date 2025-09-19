using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Items
{
    public interface IUsableItemFactory
    {
        IUsableItem? GetUsableItem( PlayerManager playerManager, string requestText );
    }
}