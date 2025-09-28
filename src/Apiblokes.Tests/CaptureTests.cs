using System.ComponentModel.DataAnnotations;
using Apiblokes.Game.Managers.Battle;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Game.Model;
using Apiblokes.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests;

public class CaptureTests
{
    private TestDataContextFactory dataContextFactory;
    private BlokeManagerBuilder blokeManagerBuilder;
    private PlayerManagerBuilder playerManagerBuilder;
    private PlayerManager playerManager;
    private Bloke bloke;

    [SetUp]
    public async Task Setup()
    {
        dataContextFactory = new TestDataContextFactory();
        blokeManagerBuilder = new BlokeManagerBuilder( dataContextFactory );
        playerManagerBuilder = new PlayerManagerBuilder( dataContextFactory, blokeManagerBuilder );

        playerManager = await playerManagerBuilder.FromNewPlayer( "Trainer" );

        var blokeManager = await blokeManagerBuilder.FromWorldSpawnAsync( playerManager.X, playerManager.Y );
        bloke = await dataContextFactory.DataContext.Blokes.FirstAsync( b => b.Id == blokeManager.Id );

    }

    [TearDown]
    public void Teardown()
    {
        dataContextFactory.Dispose();
    }

    [Test]
    public async Task CaptureBasicBloke()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.Manager;

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( bloke.Name );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 2 ) );
    }

    [Test]
    public async Task CaptureBasicBloke_OneBlokeInLocation()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.Manager;

        dataContextFactory.DataContext.SaveChanges();

        await playerManager.AttemptCaptureAsync( "WRONG NAME" );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 2 ) );
    }

    [Test]
    public async Task CaptureBasicBloke_FailWrongName()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.Manager;

        dataContextFactory.DataContext.SaveChanges();

        await blokeManagerBuilder.FromWorldSpawnAsync( playerManager.X, playerManager.Y );

        await playerManager.AttemptCaptureAsync( "WRONG NAME" );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public async Task CaptureBasicBloke_FailWrongLocations()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.Manager;

        await playerManager.MovePlayerAsync( "north" );

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( "WRONG NAME" );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public async Task CaptureLevel2Bloke()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.Developer;

        await playerManager.AddCatcherAsync( 2, 1 );

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( bloke.Name );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 2 ) );
    }

    [Test]
    public async Task CaptureLevel2Bloke_FailureNoCatcher()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.Developer;

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( bloke.Name );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public async Task CaptureLevel2BlokeWithLevel3()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.Developer;

        await playerManager.AddCatcherAsync( 3, 1 );

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( bloke.Name );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 2 ) );
    }

    [Test]
    public async Task CaptureLevel3Bloke()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.DoItAll;

        await playerManager.AddCatcherAsync( 3, 1 );

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( bloke.Name );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 2 ) );
    }

    [Test]
    public async Task CaptureLevel3Bloke_FailureNoCatcher()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.DoItAll;

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( bloke.Name );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public async Task CaptureLevel3Bloke_FailureLevel2Catcher()
    {
        bloke.Health = 0;
        bloke.Type = BlokeType.DoItAll;

        await playerManager.AddCatcherAsync( 2, 1 );

        dataContextFactory.DataContext.SaveChanges();
        await playerManager.AttemptCaptureAsync( bloke.Name );
        var blokes = await playerManager.GetPersonalBlokesAsync();

        Assert.That( blokes.Count, Is.EqualTo( 1 ) );
    }

}
