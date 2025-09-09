using System.Diagnostics.Metrics;
using Apiblokes.Game.Data;
using Apiblokes.Game.Managers;
using Apiblokes.Game.Model;
using Apiblokes.Tests.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests
{
    public class PlayerMovement
    {
        private readonly Guid PLAYER_ID = new Guid( "99eeca9c-582d-4a8b-ae51-a0cb2545bb1d" );
        private TestDataContext dataContext;
        private SqliteConnection? connection;

        [SetUp]
        public void Init()
        {

            connection = new SqliteConnection( "DataSource=:memory:" );
            connection.Open();

            var options = new DbContextOptionsBuilder<TestDataContext>()
            .UseSqlite( connection )
            .Options;

            dataContext = new TestDataContext( options );
            dataContext.Database.EnsureCreated();

            dataContext.Players.Add( new Player
            {
                Id = PLAYER_ID,
                X = 1,
                Y = 1
            }
            );
            dataContext.SaveChanges();
        }

        [TearDown]
        public void Cleanup()
        {
            connection?.Dispose();
            dataContext.Dispose();
        }

        [Test]
        public async Task Player_MoveNorth()
        {
            var playerManger = new PlayerManager( dataContext, PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "north" );

            Assert.That( dataContext.Players.First().Y, Is.EqualTo( 2 ) );
        }

        [Test]
        public async Task Player_MoveSouth()
        {
            var playerManger = new PlayerManager( dataContext, PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "south" );


            Assert.That( dataContext.Players.First().Y, Is.EqualTo( 0 ) );
        }

        [Test]
        public async Task Player_MoveEast()
        {
            var playerManger = new PlayerManager( dataContext, PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "east" );


            Assert.That( dataContext.Players.First().X, Is.EqualTo( 2 ) );
        }

        [Test]
        public async Task Player_MoveWest()
        {
            var playerManger = new PlayerManager( dataContext, PLAYER_ID.ToString() );
            await playerManger.MovePlayerAsync( "west" );


            Assert.That( dataContext.Players.First().X, Is.EqualTo( 0 ) );
        }
    }
}
