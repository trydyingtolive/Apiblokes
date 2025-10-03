using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests;

public class BlokeTests
{
    TestDataContextFactory testDataContextFactory;

    [SetUp]
    public async Task Setup()
    {
        testDataContextFactory = new TestDataContextFactory();

    }

    [TearDown]
    public void Teardown()
    {
        testDataContextFactory.Dispose();
    }


    [Test]
    public async Task PopulateBlokes()
    {
        TestDataContextFactory dataContextFactory = new TestDataContextFactory();

        var worldPopulationManager = new WorldPopulationManager( new BlokeManagerBuilder( dataContextFactory ) );

        await worldPopulationManager.RefreshBlokesAsync();
        var blokes = await dataContextFactory.DataContext.Blokes.ToListAsync();
        Assert.That( dataContextFactory.DataContext.Blokes.Count(), Is.EqualTo( Constants.MaxNumberOfWorldBlokes ) );
    }

    [Test]
    public async Task BlokeLevelUp()
    {
        var builder = new BlokeManagerBuilder( testDataContextFactory );
        var blokeManager = await builder.FromWorldSpawnAsync( 0, 0 );

        var baseBloke = testDataContextFactory.DataContext.Blokes.First();
        var health = baseBloke.Health;
        var damage = baseBloke.Damage;
        var hit = baseBloke.HitProbability;

        await blokeManager.AddExperienceAsync( 1 );

        var bloke = testDataContextFactory.DataContext.Blokes.First();

        Assert.That( bloke.Experience, Is.EqualTo( 1 ) );

        var somethingWasUpdated = false;

        if (baseBloke.MaxHealth != health
            || baseBloke.Damage != damage 
            || baseBloke.HitProbability != hit )
        {
            somethingWasUpdated = true;
        }

        Assert.That(somethingWasUpdated, Is.True );
    }

    [Test]
    public async Task BlokeNotLevelUpAt4()
    {
        var builder = new BlokeManagerBuilder( testDataContextFactory );
        var blokeManager = await builder.FromWorldSpawnAsync( 0, 0 );

        await blokeManager.AddExperienceAsync( 3 );

        var baseBloke = testDataContextFactory.DataContext.Blokes.First();
        var health = baseBloke.Health;
        var damage = baseBloke.Damage;
        var hit = baseBloke.HitProbability;

        await blokeManager.AddExperienceAsync( 1 );

        var bloke = testDataContextFactory.DataContext.Blokes.First();

        Assert.That( bloke.Experience, Is.EqualTo( 4 ) );

        var somethingWasUpdated = false;

        if ( baseBloke.MaxHealth == health
            && baseBloke.Damage == damage
            && baseBloke.HitProbability == hit )
        {
            somethingWasUpdated = true;
        }

        Assert.That( somethingWasUpdated, Is.True );
    }

}
