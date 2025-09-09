using Apiblokes.Game.Managers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests.Helpers
{
    internal class TestGameManager : IGameManager, IDisposable
    {
        public TestDataContext DataContext { get; set; }
        private SqliteConnection connection;

        public TestGameManager()
        {
            connection = new SqliteConnection( "DataSource=:memory:" );
            connection.Open();

            var options = new DbContextOptionsBuilder<TestDataContext>()
            .UseSqlite( connection )
            .Options;

            DataContext = new TestDataContext( options );
            DataContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            connection?.Dispose();
            DataContext.Dispose();
        }

        public async Task<PlayerManager> GetPlayerManagerAsync( string playerId )
        {
            var playerManager = new PlayerManager( DataContext );
            return await playerManager.GetPlayerAsync( playerId );
        }
    }
}
