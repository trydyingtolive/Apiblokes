using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests;

public class BlokeTests
{
    [Test]
    public async Task PopulateBlokes()
    {
        TestDataContextFactory dataContextFactory = new TestDataContextFactory();

        var worldPopulationManager = new WorldPopulationManager( new BlokeManagerBuilder( dataContextFactory ) );

        await worldPopulationManager.RefreshBlokesAsync();
        var blokes = await dataContextFactory.DataContext.Blokes.ToListAsync();
        Assert.That( dataContextFactory.DataContext.Blokes.Count(), Is.EqualTo( Constants.MaxNumberOfWorldBlokes ) );
    }

}
