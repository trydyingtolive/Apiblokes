using Apiblokes.Game.Data;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Game.Managers.Players;

public class PlayerManagerBuilder : IPlayerManagerBuilder
{
    private readonly IDataContextFactory dataContextFactory;
    private readonly IBlokeManagerBuilder blokeManagerBuilder;
    public PlayerManagerBuilder( IDataContextFactory dataContextFactory, IBlokeManagerBuilder blokeManagerBuilder )
    {
        this.dataContextFactory = dataContextFactory;
        this.blokeManagerBuilder = blokeManagerBuilder;
    }

    public async Task<PlayerManager?> FromKeyAsync( string passKey )
    {
        var dataContext = dataContextFactory.CreateContext();
        var passKeyUpper = passKey.ToUpper();
        var player = await dataContext.Players.FirstOrDefaultAsync( p => p.PassKey == passKeyUpper );

        if ( player == null )
        {
            return default;
        }

        return new PlayerManager( player, dataContext, blokeManagerBuilder );
    }


    public async Task<PlayerManager> FromNewPlayer( string name )
    {
        var dataContext = dataContextFactory.CreateContext();

        var player = new Player()
        {
            Name = name
        };

        dataContext.Players.Add( player );
        await dataContext.SaveChangesAsync();

        await blokeManagerBuilder.FromStarterAsync( player.Id );

        return new PlayerManager( player, dataContext, blokeManagerBuilder );
    }
}
