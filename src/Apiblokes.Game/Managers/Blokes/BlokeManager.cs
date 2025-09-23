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

    public async Task<int> FireBlokeAsync()
    {
        dataContext.Blokes.Remove( bloke );
        await dataContext.SaveChangesAsync();

        //how much money each one returns
        switch ( Type )
        {
            case BlokeType.Manager:
                return 1;
            case BlokeType.HelpDesk:
            case BlokeType.Network:
            case BlokeType.SystemAdmin:
            case BlokeType.Developer:
                return 5;
            case BlokeType.DoItAll:
                return 25;
            default:
                return 1;
        }

    }

    public async Task TakeDamageAsync( int damage )
    {
        bloke.Health -= damage;
        bloke.Health = Math.Max( bloke.Health, 0 );
        await dataContext.SaveChangesAsync();
    }

    public async Task<int> RestoreHealthAsync()
    {
        bloke.Health = bloke.MaxHealth;
        await dataContext.SaveChangesAsync();
        return bloke.Health;
    }

    public async Task<string?> AddExperienceAsync( int hp )
    {
        bloke.Experience += hp;
        await dataContext.SaveChangesAsync();

        if ( ExperienceIsLevelUp( hp ) )
        {
            return await LevelUpBlokeAsync();
        }
        return default;
    }

    private async Task<string?> LevelUpBlokeAsync()
    {
        var r = new Random();
        switch ( r.Next( 0, 3 ) )
        {
            case 0:
                bloke.Health += 5;
                bloke.MaxHealth += 5;
                await dataContext.SaveChangesAsync();
                return $"{bloke.Name} gained 5 max health. ({bloke.MaxHealth})";
            case 1:
                bloke.Damage += 2;
                await dataContext.SaveChangesAsync();
                return $"{bloke.Name} gained 2 damage ({bloke.Damage})";
            case 2:
                bloke.HitProbability += 0.5f;
                await dataContext.SaveChangesAsync();
                return $"{bloke.Name} gained .5 hit chance. {bloke.HitProbability}";
            default:
                return default;
        }

    }


    //Level ups happen on a Fibonacci scale --fast then slow
    private bool ExperienceIsLevelUp( int hp, int a = 1, int b = 2 )
    {
        if ( hp == a )
        {
            return true;
        }

        if ( hp < a )
        {
            return false;
        }

        return ExperienceIsLevelUp( hp, b, a + b );
    }
}
