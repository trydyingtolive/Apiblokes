using Apiblokes.Game.Helpers;
using Microsoft.Extensions.Hosting;

namespace Apiblokes.Game.Managers.Blokes;

public class WorldPopulationManager : BackgroundService
{
    private readonly IBlokeManagerBuilder blokeManagerBuilder;
    public WorldPopulationManager( IBlokeManagerBuilder blokeManagerBuilder )
    {
        this.blokeManagerBuilder = blokeManagerBuilder;
    }
    protected override async Task ExecuteAsync( CancellationToken stoppingToken )
    {
        while ( true )
        {
            await RefreshBlokesAsync();
            await Task.Delay( 60 * 1000 );
        }
    }

    public async Task RefreshBlokesAsync()
    {
        await RemoveOldBlokesAsync();
        await PopulateBlokesAsync();
    }



    private async Task RemoveOldBlokesAsync()
    {
        var worldBlokes = await blokeManagerBuilder.AllFromWorldMapAsync();

        var hourAgo = DateTime.UtcNow.AddHours( -1 );

        foreach ( var bloke in worldBlokes )
        {
            if ( bloke.CreatedDateTime < hourAgo )
            {
                await bloke.FireBlokeAsync();
            }
        }
    }

    private async Task PopulateBlokesAsync()
    {
        var numberOfBlokes = ( await blokeManagerBuilder.AllFromWorldMapAsync() ).Count();
        Random r = new Random();

        while ( numberOfBlokes < Constants.MaxNumberOfWorldBlokes )
        {
            await blokeManagerBuilder.FromWorldSpawn( r.Next( Constants.XMinimum, Constants.XMaximum + 1 ), r.Next( Constants.YMinimum, Constants.YMaximum + 1 ) );
            numberOfBlokes++;
        }

    }
}
