using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Game.Model;
using Apiblokes.Tests.Helpers;

namespace Apiblokes.Tests
{
    public class PlayerMovementTests
    {
        private string PassKey;
        private TestDataContextFactory dataContextFactory;
        private PlayerManagerBuilder playerManagerBuilder;
        private BlokeManagerBuilder blokeManagerBuilder;


        [SetUp]
        public void Init()
        {
            dataContextFactory = new TestDataContextFactory();
            blokeManagerBuilder = new BlokeManagerBuilder( dataContextFactory );
            playerManagerBuilder = new PlayerManagerBuilder( dataContextFactory, blokeManagerBuilder );

            var player = new Player
            {
                X = 1,
                Y = 1,
            };

            PassKey = player.PassKey;

            dataContextFactory.DataContext.Players.Add( player );
            dataContextFactory.DataContext.SaveChanges();
        }

        [TearDown]
        public void Cleanup()
        {
            dataContextFactory.Dispose();
        }

        [Test]
        public async Task Player_MoveNorth()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );
            await playerManager!.MovePlayerAsync( "north" );

            Assert.That( dataContextFactory.DataContext.Players.First().Y, Is.EqualTo( 2 ) );
        }

        [Test]
        public async Task Player_MoveNorthMax()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManager!.MovePlayerAsync( "north" );
            }

            Assert.That( dataContextFactory.DataContext.Players.First().Y, Is.EqualTo( 10 ) );
        }

        [Test]
        public async Task Player_MoveSouth()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );
            await playerManager!.MovePlayerAsync( "south" );


            Assert.That( dataContextFactory.DataContext.Players.First().Y, Is.EqualTo( 0 ) );
        }

        [Test]
        public async Task Player_MoveSouthMax()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManager!.MovePlayerAsync( "south" );
            }

            Assert.That( dataContextFactory.DataContext.Players.First().Y, Is.EqualTo( 0 ) );
        }

        [Test]
        public async Task Player_MoveEast()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );
            await playerManager!.MovePlayerAsync( "east" );


            Assert.That( dataContextFactory.DataContext.Players.First().X, Is.EqualTo( 2 ) );
        }

        [Test]
        public async Task Player_MoveEastMax()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManager!.MovePlayerAsync( "east" );
            }

            Assert.That( dataContextFactory.DataContext.Players.First().X, Is.EqualTo( 10 ) );
        }

        [Test]
        public async Task Player_MoveWest()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );
            await playerManager!.MovePlayerAsync( "west" );


            Assert.That( dataContextFactory.DataContext.Players.First().X, Is.EqualTo( 0 ) );
        }

        [Test]
        public async Task Player_MoveWestMax()
        {
            var playerManager = await playerManagerBuilder.FromKeyAsync( PassKey );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManager!.MovePlayerAsync( "west" );
            }

            Assert.That( dataContextFactory.DataContext.Players.First().X, Is.EqualTo( 0 ) );
        }
    }
}
