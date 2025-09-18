using Apiblokes.Game.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests.Helpers;

public class TestDataContextFactory : IDataContextFactory, IDisposable
{
    public TestDataContext DataContext { get; set; }
    private SqliteConnection connection;

    public TestDataContextFactory()
    {
        connection = new SqliteConnection( "DataSource=:memory:" );
        connection.Open();

        var options = new DbContextOptionsBuilder<TestDataContext>()
        .UseSqlite( connection )
        .Options;

        DataContext = new TestDataContext( options );
        DataContext.Database.EnsureCreated();
    }

    public IDataContext CreateContext()
    {
        return DataContext;
    }

    public void Dispose()
    {
        connection?.Dispose();
        DataContext.Dispose();
    }
}
