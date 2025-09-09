
namespace Apiblokes.Game.Managers
{
    public interface IGameManager
    {
        Task<PlayerManager> GetPlayerManagerAsync( string playerId );
    }
}