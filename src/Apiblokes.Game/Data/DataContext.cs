using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Game.Data;

public class DataContext : DbContext, IDataContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Bloke> Blokes { get; set; }

    public string DbPath { get; }

    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath( folder );
        DbPath = System.IO.Path.Join( path, "apiblokes.db" );
    }

    protected override void OnConfiguring( DbContextOptionsBuilder options )
    => options.UseSqlite( $"Data Source={DbPath}" );

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}

