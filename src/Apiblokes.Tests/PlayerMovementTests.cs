using Apiblokes.Game.Model;
using Apiblokes.Tests.Helpers;

namespace Apiblokes.Tests
{
    public class PlayerMovementTests
    {
        private readonly Guid PLAYER_ID = new Guid( "99eeca9c-582d-4a8b-ae51-a0cb2545bb1d" );
        private TestGameManager gameManager;


        [SetUp]
        public void Init()
        {
            gameManager = new TestGameManager();

            gameManager.DataContext.Players.Add( new Player
            {
                Id = PLAYER_ID,
                X = 1,
                Y = 1
            }
            );
            gameManager.DataContext.SaveChanges();
        }

        [TearDown]
        public void Cleanup()
        {
            gameManager.Dispose();
        }

        [Test]
        public async Task Player_MoveNorth()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "north" );

            Assert.That( gameManager.DataContext.Players.First().Y, Is.EqualTo( 2 ) );
        }

        [Test]
        public async Task Player_MoveNorthMax()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManger.MovePlayerAsync( "north" );
            }

            Assert.That( gameManager.DataContext.Players.First().Y, Is.EqualTo( 10 ) );
        }

        [Test]
        public async Task Player_MoveSouth()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "south" );


            Assert.That( gameManager.DataContext.Players.First().Y, Is.EqualTo( 0 ) );
        }

        [Test]
        public async Task Player_MoveSouthMax()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManger.MovePlayerAsync( "south" );
            }

            Assert.That( gameManager.DataContext.Players.First().Y, Is.EqualTo( 0 ) );
        }

        [Test]
        public async Task Player_MoveEast()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "east" );


            Assert.That( gameManager.DataContext.Players.First().X, Is.EqualTo( 2 ) );
        }

        [Test]
        public async Task Player_MoveEastMax()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManger.MovePlayerAsync( "east" );
            }

            Assert.That( gameManager.DataContext.Players.First().X, Is.EqualTo( 10 ) );
        }

        [Test]
        public async Task Player_MoveWest()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "west" );


            Assert.That( gameManager.DataContext.Players.First().X, Is.EqualTo( 0 ) );
        }

        [Test]
        public async Task Player_MoveWestMax()
        {
            var playerManger = await gameManager.GetPlayerManagerAsync( PLAYER_ID.ToString() );

            for ( int i = 0; i < 10; i++ )
            {
                await playerManger.MovePlayerAsync( "west" );
            }

            Assert.That( gameManager.DataContext.Players.First().X, Is.EqualTo( 0 ) );
        }
    }
}
