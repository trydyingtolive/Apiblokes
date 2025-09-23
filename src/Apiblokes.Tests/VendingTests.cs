using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Items;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Game.Model;
using Apiblokes.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests;

public class VendingTests
{
    private TestDataContextFactory dataContextFactory;
    private PlayerManagerBuilder playerManagerBuilder;
    private BlokeManagerBuilder blokeManagerBuilder;

    [SetUp]
    public void Setup()
    {
        dataContextFactory = new TestDataContextFactory();

        blokeManagerBuilder = new BlokeManagerBuilder( dataContextFactory );
        playerManagerBuilder = new PlayerManagerBuilder( dataContextFactory, blokeManagerBuilder );
    }

    [TearDown]
    public void Teardown()
    {
        dataContextFactory.Dispose();
    }

    [Test]
    public async Task WrongLocation()
    {
        var player = new Player
        {
            X = 1,
            Y = 1,
            Money = 100
        };
        dataContextFactory.DataContext.Players.Add( player );
        var manager = await playerManagerBuilder.FromKeyAsync( player.PassKey );

        var factory = new UsableItemFactory();
        var vending = factory.GetUsableItem( manager!, "vending" );

        Assert.That( vending, Is.Not.Null );


    }

    [Test]
    public async Task GetCubical()
    {
        var player = new Player
        {
            X = Constants.XMinimum,
            Y = Constants.YMaximum,
            Money = 100
        };

        dataContextFactory.DataContext.Players.Add( player );
        await dataContextFactory.DataContext.SaveChangesAsync();

        var manager = await playerManagerBuilder.FromKeyAsync( player.PassKey );

        var factory = new UsableItemFactory();
        var vending = factory.GetUsableItem( manager!, "vending" );

        Assert.That( vending, Is.Not.Null );

        await vending.UseItemAsync();

        var updatedPlayer = await dataContextFactory.DataContext.Players.FirstAsync();
        Assert.That( updatedPlayer.Money, Is.EqualTo( 90 ) );
        Assert.That( updatedPlayer.Level2Catchers, Is.EqualTo( 1 ) );
    }

    [Test]
    public async Task GetOffice()
    {
        var player = new Player
        {
            X = Constants.XMaximum,
            Y = Constants.YMinimum,
            Money = 100
        };

        dataContextFactory.DataContext.Players.Add( player );
        await dataContextFactory.DataContext.SaveChangesAsync();

        var manager = await playerManagerBuilder.FromKeyAsync( player.PassKey );

        var factory = new UsableItemFactory();
        var vending = factory.GetUsableItem( manager!, "vending" );

        Assert.That( vending, Is.Not.Null );

        await vending.UseItemAsync();

        var updatedPlayer = await dataContextFactory.DataContext.Players.FirstAsync();
        Assert.That( updatedPlayer.Money, Is.EqualTo( 50 ) );
        Assert.That( updatedPlayer.Level3Catchers, Is.EqualTo( 1 ) );
    }

    [Test]
    public async Task TryGetCubical_InsufficientFunds()
    {
        var player = new Player
        {
            X = Constants.XMinimum,
            Y = Constants.YMaximum,
            Money = 1
        };

        dataContextFactory.DataContext.Players.Add( player );
        await dataContextFactory.DataContext.SaveChangesAsync();

        var manager = await playerManagerBuilder.FromKeyAsync( player.PassKey );

        var factory = new UsableItemFactory();
        var vending = factory.GetUsableItem( manager!, "vending" );

        Assert.That( vending, Is.Not.Null );

        await vending.UseItemAsync();

        var updatedPlayer = await dataContextFactory.DataContext.Players.FirstAsync();
        Assert.That( updatedPlayer.Money, Is.EqualTo( 1 ) );
        Assert.That( updatedPlayer.Level2Catchers, Is.EqualTo( 0 ) );
    }
}
