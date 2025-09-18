using Apiblokes.Game.Data;
using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Tests.Helpers;

public class TestDataContext : DbContext, IDataContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Bloke> Blokes { get; set; }

    public TestDataContext( DbContextOptions<TestDataContext> options ) : base( options )
    {
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}
