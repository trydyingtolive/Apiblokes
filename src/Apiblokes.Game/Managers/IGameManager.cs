
namespace Apiblokes.Game.Managers
{
    public interface IGameManager
    {
        BlokeManager GetBlokeManager();
        Task<PlayerManager> GetPlayerManagerAsync( string playerId );
    }
}