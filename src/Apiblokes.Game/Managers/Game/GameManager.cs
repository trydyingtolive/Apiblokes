using Apiblokes.Game.Data;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Game.Managers.Game;

public class GameManager : IGameManager
{
    private readonly IDataContext context;
    private readonly IBlokeManagerBuilder blokeManagerBuilder;

    public GameManager( IDataContext context, IBlokeManagerBuilder blokeManagerBuilder )
    {
        this.context = context;
        this.blokeManagerBuilder = blokeManagerBuilder;
    }

    public async Task<PlayerManager> GetPlayerManagerAsync( string playerId )
    {
        return await new PlayerManager( context, blokeManagerBuilder ).GetPlayerAsync( playerId );
    }

}
