using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers;
using Apiblokes.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests;

public class BlokeTests
{
    [Test]
    public void CreateBlokeName()
    {
        var blokeName = BlokeManager.GenerateBlokeName();
        Assert.That( blokeName, Is.Not.Null );
    }


    [Test]
    public async Task PopulateBlokes()
    {
        var testManager = new TestGameManager();
        var blokeManager = testManager.GetBlokeManager();
        await blokeManager.RefreshBlokes();
        var blokes = await testManager.DataContext.Blokes.ToListAsync();
        Assert.That( testManager.DataContext.Blokes.Count(), Is.EqualTo( Constants.MaxNumberOfWorldBlokes ) );
    }

}
