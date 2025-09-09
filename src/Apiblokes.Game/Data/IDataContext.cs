using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Game.Data;

public interface IDataContext
{
    DbSet<Player> Players { get; set; }
    Task<int> SaveChangesAsync();
}