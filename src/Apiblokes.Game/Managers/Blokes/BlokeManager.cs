using Apiblokes.Game.Data;
using Apiblokes.Game.Helpers;
using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

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


    public async Task FireBlokeAsync()
    {
        dataContext.Blokes.Remove( bloke );
        await dataContext.SaveChangesAsync();
    }
}
