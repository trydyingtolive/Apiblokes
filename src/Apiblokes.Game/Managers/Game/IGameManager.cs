using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Game
{
    public interface IGameManager
    {
        Task<PlayerManager> GetPlayerManagerAsync( string playerId );
    }
}