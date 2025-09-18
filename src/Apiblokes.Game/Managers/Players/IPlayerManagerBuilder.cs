
namespace Apiblokes.Game.Managers.Players
{
    public interface IPlayerManagerBuilder
    {
        Task<PlayerManager?> FromKeyAsync( string passKey );
        Task<PlayerManager> FromNewPlayer( string name );
    }
}