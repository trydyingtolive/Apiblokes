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
    public string Type { get => bloke.Type.ToString(); }
    public int Health { get => bloke.Health; }

    public async Task FireBlokeAsync()
    {
        dataContext.Blokes.Remove( bloke );
        await dataContext.SaveChangesAsync();
    }
}
