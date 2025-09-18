using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Game;
using Apiblokes.Game.Managers.Players;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests.Helpers
{
    public class TestGameManager : IGameManager, IDisposable
    {
        
        private IBlokeManagerBuilder blokeManagerBuilder;
        public TestDataContextFactory DataContextFactory;

        public TestDataContext DataContext { get => DataContextFactory.DataContext; }


        public TestGameManager()
        {
            DataContextFactory = new TestDataContextFactory();
            blokeManagerBuilder = new BlokeManagerBuilder( DataContextFactory );
        }

        public void Dispose()
        {
      
        }

        public async Task<PlayerManager> GetPlayerManagerAsync( string playerId )
        {
            var playerManager = new PlayerManager( DataContext, blokeManagerBuilder );
            return await playerManager.GetPlayerAsync( playerId );
        }

    }
}
