using Apiblokes.Game.Managers.Battle;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Game.Model;
using Apiblokes.Tests.Helpers;

namespace Apiblokes.Tests;

public class BattleTests
{
    private TestDataContextFactory dataContextFactory;
    private BlokeManagerBuilder blokeManagerBuilder;
    private PlayerManagerBuilder playerManagerBuilder;
    private PlayerManager playerManager;

    [SetUp]
    public async Task Setup()
    {
        dataContextFactory = new TestDataContextFactory();
        blokeManagerBuilder = new BlokeManagerBuilder( dataContextFactory );
        playerManagerBuilder = new PlayerManagerBuilder( dataContextFactory, blokeManagerBuilder );

        playerManager = await playerManagerBuilder.FromNewPlayer( "Trainer" );
    }

    [TearDown]
    public void Teardown()
    {
        dataContextFactory.Dispose();
    }

    [Test]
    public async Task CreateValidBattle()
    {
        dataContextFactory.DataContext.Blokes.Add( new Bloke
        {
            Name = "Defender",
            Damage = 1,
            Health = 10,
            Type = BlokeType.DoItAll,
            HitProbability = 1,
            X = playerManager.X,
            Y = playerManager.Y,
        } );

        await dataContextFactory.DataContext.SaveChangesAsync();

        var availableBlokes = await blokeManagerBuilder.AllFromPlayerInventory( playerManager.Id );

        var options = new BattleRequestOptions
        {
            AvailablePlayerBlokes = availableBlokes,
            RequestText = $"Defender with {availableBlokes.First().Name}",
            X = playerManager.X,
            Y = playerManager.Y
        };
        var output = await BattleManager.SetupBattleAsync( blokeManagerBuilder, options );

        Assert.That( output.Item1, Is.Not.Null );
    }

    [Test]
    public async Task CreateValidBattle_AND_ProcessBattle()
    {
        dataContextFactory.DataContext.Blokes.Add( new Bloke
        {
            Name = "Defender",
            Damage = 1,
            Health = 10,
            Type = BlokeType.DoItAll,
            HitProbability = 1,
            X = playerManager.X,
            Y = playerManager.Y,
        } );

        await dataContextFactory.DataContext.SaveChangesAsync();

        var availableBlokes = await blokeManagerBuilder.AllFromPlayerInventory( playerManager.Id );

        var options = new BattleRequestOptions
        {
            AvailablePlayerBlokes = availableBlokes,
            RequestText = $"Defender with {availableBlokes.First().Name}",
            X = playerManager.X,
            Y = playerManager.Y
        };
        var output = await BattleManager.SetupBattleAsync( blokeManagerBuilder, options );

        var battleText = await output!.Item1!.ProcessBattleAsync(); 
    }

    [Test]
    public async Task CreateBattleFail_BlokeNotInSamePlace()
    {
        dataContextFactory.DataContext.Blokes.Add( new Bloke
        {
            Name = "Defender",
            Damage = 1,
            Health = 10,
            Type = BlokeType.DoItAll,
            HitProbability = 1,
            X = playerManager.X+1,
            Y = playerManager.Y+1,
        } );

        await dataContextFactory.DataContext.SaveChangesAsync();

        var availableBlokes = await blokeManagerBuilder.AllFromPlayerInventory( playerManager.Id );

        var options = new BattleRequestOptions
        {
            AvailablePlayerBlokes = availableBlokes,
            RequestText = $"Defender with {availableBlokes.First().Name}",
            X = playerManager.X,
            Y = playerManager.Y
        };
        var output = await BattleManager.SetupBattleAsync( blokeManagerBuilder, options );

        Assert.That( output.Item1, Is.Null );
    }

    [Test]
    public async Task CreateBattleFail_SwitchedNames()
    {
        dataContextFactory.DataContext.Blokes.Add( new Bloke
        {
            Name = "Defender",
            Damage = 1,
            Health = 10,
            Type = BlokeType.DoItAll,
            HitProbability = 1,
            X = playerManager.X + 1,
            Y = playerManager.Y + 1,
        } );

        await dataContextFactory.DataContext.SaveChangesAsync();

        var availableBlokes = await blokeManagerBuilder.AllFromPlayerInventory( playerManager.Id );

        var options = new BattleRequestOptions
        {
            AvailablePlayerBlokes = availableBlokes,
            RequestText = $"{availableBlokes.First().Name} with defender",
            X = playerManager.X,
            Y = playerManager.Y
        };
        var output = await BattleManager.SetupBattleAsync( blokeManagerBuilder, options );

        Assert.That( output.Item1, Is.Null );
    }

}
