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
        var testManager = new TestGameManager();

        var worldPopulationManager = new WorldPopulationManager( new BlokeManagerBuilder( testManager.DataContext ) );

        await worldPopulationManager.RefreshBlokesAsync();
        var blokes = await testManager.DataContext.Blokes.ToListAsync();
        Assert.That( testManager.DataContext.Blokes.Count(), Is.EqualTo( Constants.MaxNumberOfWorldBlokes ) );
    }

}
