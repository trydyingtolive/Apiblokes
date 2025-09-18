using System.Threading.Tasks;
using Apiblokes.Game.Data;
using Apiblokes.Game.Model;

namespace Apiblokes.Game.Managers.Blokes;

public class BlokeManager
{
    private readonly IDataContext dataContext;
    private readonly Bloke bloke;
    public BlokeManager( IDataContext dataContext, Bloke bloke )
    {
        this.dataContext = dataContext;
        this.bloke = bloke;
    }

    public DateTime CreatedDateTime { get => bloke.CreatedDateTime; }
    public string Name { get => bloke.Name; }
    public BlokeType Type { get => bloke.Type; }
    public int Health { get => bloke.Health; }
    public double HitProbability { get => bloke.HitProbability; }
    public int Damage { get => bloke.Damage; }

    public async Task FireBlokeAsync()
    {
        dataContext.Blokes.Remove( bloke );
        await dataContext.SaveChangesAsync();
    }

    public async Task TakeDamageAsync( int damage )
    {
        bloke.Health -= damage;
        bloke.Health = Math.Max( bloke.Health, 0 );
        await dataContext.SaveChangesAsync();
    }
}
