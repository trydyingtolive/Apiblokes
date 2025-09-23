using Apiblokes.Game.Data;
using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Game.Managers.Blokes;

public class BlokeManagerBuilder : IBlokeManagerBuilder
{
    private readonly IDataContextFactory dataContextFactory;

    public BlokeManagerBuilder( IDataContextFactory dataContextFactory )
    {
        this.dataContextFactory = dataContextFactory;
    }

    public async Task<BlokeManager?> FromId( string id )
    {
        if ( !Guid.TryParse( id, out var blokeId ) )
        {
            return default;
        }

        var dataContext = dataContextFactory.CreateContext();
        var bloke = await dataContext.Blokes.FirstOrDefaultAsync( b => b.Id == blokeId );

        if ( bloke == null )
        {
            return default;
        }

        return new BlokeManager( dataContext, bloke );
    }

    public async Task<BlokeManager> FromStarterAsync( Guid playerId )
    {
        var bloke = BlokeCreator.CreateStarterBloke( playerId );

        var dataContext = dataContextFactory.CreateContext();
        dataContext.Blokes.Add( bloke );
        await dataContext.SaveChangesAsync();

        return new BlokeManager( dataContext, bloke );
    }

    public async Task<BlokeManager> FromWorldSpawnAsync( int x, int y )
    {
        var bloke = BlokeCreator.CreateBloke( x, y );

        var dataContext = dataContextFactory.CreateContext();
        dataContext.Blokes.Add( bloke );
        await dataContext.SaveChangesAsync();

        return new BlokeManager( dataContext, bloke );
    }

    public async Task<List<BlokeManager>> AllFromWorldMapAsync()
    {
        var dataContext = dataContextFactory.CreateContext();
        var blokes = await dataContext.Blokes.Where( b => b.PlayerId == null ).ToListAsync();

        return blokes
            .Select( b => new BlokeManager( dataContext, b ) )
            .ToList();
    }

    public async Task<List<BlokeManager>> AllFromPlayerInventory( Guid playerId )
    {
        var dataContext = dataContextFactory.CreateContext();
        var blokes = await dataContext.Blokes.Where( b => b.PlayerId == playerId ).ToListAsync();

        return blokes
            .Select( b => new BlokeManager( dataContext, b ) )
            .ToList();
    }

    public async Task<List<BlokeManager>> AllFromWorldLocation( int x, int y )
    {
        var dataContext = dataContextFactory.CreateContext();
        var blokes = await dataContext.Blokes.Where( b => b.PlayerId == null && b.X == x && b.Y == y ).ToListAsync();

        return blokes
            .Select( b => new BlokeManager( dataContext, b ) )
            .ToList();
    }
}
