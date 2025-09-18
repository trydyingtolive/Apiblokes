
namespace Apiblokes.Game.Managers.Blokes
{
    public interface IBlokeManagerBuilder
    {
        Task<List<BlokeManager>> AllFromPlayerInventory( Guid playerId );
        Task<List<BlokeManager>> AllFromWorldLocation( int x, int y );
        Task<List<BlokeManager>> AllFromWorldMapAsync();
        Task<BlokeManager?> FromId( string id );
        Task<BlokeManager> FromStarterAsync( Guid playerId );
        Task<BlokeManager> FromWorldSpawn( int x, int y );
    }
}