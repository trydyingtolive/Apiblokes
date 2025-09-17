using Apiblokes.Game.Data;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Game.Managers.Blokes;

public class BlokeManagerBuilder : IBlokeManagerBuilder
{
    private readonly IDataContext dataContext;

    public BlokeManagerBuilder( IDataContext dataContext )
    {
        this.dataContext = dataContext;
    }

    public async Task<BlokeManager?> FromId( string id )
    {
        if ( !Guid.TryParse( id, out var blokeId ) )
        {
            return default;
        }

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

        dataContext.Blokes.Add( bloke );
        await dataContext.SaveChangesAsync();

        return new BlokeManager( dataContext, bloke );
    }

    public async Task<BlokeManager> FromWorldSpawn( int x, int y )
    {
        var bloke = BlokeCreator.CreateBloke( x, y );

        dataContext.Blokes.Add( bloke );
        await dataContext.SaveChangesAsync();

        return new BlokeManager( dataContext, bloke );
    }

    public async Task<List<BlokeManager>> AllFromWorldMapAsync()
    {
        var blokes = await dataContext.Blokes.Where( b => b.PlayerId == null ).ToListAsync();

        return blokes
            .Select( b => new BlokeManager( dataContext, b ) )
            .ToList();
    }

    public async Task<List<BlokeManager>> AllFromPlayerInventory( Guid playerId )
    {
        var blokes = await dataContext.Blokes.Where( b => b.PlayerId == playerId ).ToListAsync();

        return blokes
            .Select( b => new BlokeManager( dataContext, b ) )
            .ToList();
    }
}
