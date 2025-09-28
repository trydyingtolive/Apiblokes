
namespace Apiblokes.Game.Managers.Blokes
{
    public interface IBlokeManagerBuilder
    {
        Task<List<BlokeManager>> AllFromPlayerInventory( Guid playerId );
        Task<List<BlokeManager>> AllFromWorldLocationAsync( int x, int y );
        Task<List<BlokeManager>> AllFromWorldMapAsync();
        Task<BlokeManager?> FromId( string id );
        Task<BlokeManager> FromStarterAsync( Guid playerId );
        Task<BlokeManager> FromWorldSpawnAsync( int x, int y );
    }
}