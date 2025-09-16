using Apiblokes.Game.Data;

namespace Apiblokes.Game.Managers;

public class GameManager : IGameManager
{
    private readonly IDataContext context;

    public GameManager( IDataContext context )
    {
        this.context = context;
    }

    public BlokeManager GetBlokeManager()
    {
        return new BlokeManager( context );
    }

    public async Task<PlayerManager> GetPlayerManagerAsync( string playerId )
    {
        return await new PlayerManager( context ).GetPlayerAsync( playerId );
    }
}
