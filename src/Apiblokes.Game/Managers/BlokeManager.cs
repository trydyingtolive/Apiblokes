using Apiblokes.Game.Data;
using Apiblokes.Game.Helpers;
using Apiblokes.Game.Model;
using Microsoft.EntityFrameworkCore;

namespace Apiblokes.Game.Managers;

public class BlokeManager
{
    private readonly IDataContext dataContext;
    private Bloke? bloke;
    public BlokeManager( IDataContext dataContext )
    {
        this.dataContext = dataContext;
    }

    public async Task RefreshBlokes()
    {
        await RemoveOldBlokesAsync();
        await PopulateBlokesAsync();
    }

    private async Task PopulateBlokesAsync()
    {
        var numberOfBlokes = await dataContext.Blokes.Where( b => b.PlayerId == null).CountAsync();
        Random r = new Random();

        while ( numberOfBlokes < Constants.MaxNumberOfWorldBlokes )
        {
            var bloke = GenerateNewBloke( r.Next( Constants.XMinimum, Constants.XMaximum + 1 ), r.Next( Constants.YMinimum, Constants.YMaximum + 1 ) );
            dataContext.Blokes.Add( bloke );
            numberOfBlokes++;
        }

        await dataContext.SaveChangesAsync();
    }

    private async Task RemoveOldBlokesAsync()
    {
        var hourAgo = DateTime.UtcNow.AddHours( -1 );
        var blokes = await dataContext.Blokes.Where( b => b.PlayerId == null && b.CreatedDateTime < hourAgo ).ToListAsync();
        dataContext.Blokes.RemoveRange( blokes );
        await dataContext.SaveChangesAsync();
    }

    public async Task CreateNewBlokeAsync( int locationX, int locationY )
    {
        bloke = GenerateNewBloke( locationX, locationY );
        dataContext.Blokes.Add( bloke );

        await dataContext.SaveChangesAsync();
    }


    public static string GenerateBlokeName()
    {
        Random r = new Random();

        var firstName = NamesList.FirstNames[r.Next( 0, NamesList.FirstNames.Length )].Capitalize();
        var lastName = NamesList.LastNames[r.Next( 0, NamesList.LastNames.Length )].Capitalize();

        return firstName + " " + lastName;
    }

    public static Bloke GenerateNewBloke( int locationX, int locationY )
    {
        var bloke = new Bloke
        {
            Name = GenerateBlokeName(),
            Type = GetBlokeType( locationX, locationY ),
            X = locationX,
            Y = locationY
        };

        switch ( bloke.Type )
        {
            case BlokeType.Manager: //pretty common pretty bad
                bloke.Damage = 1;
                bloke.Health = 10;
                bloke.HitProbability = 0.3f;
                break;
            case BlokeType.HelpDesk: //Tanky but ok hit prob
                bloke.Damage = 5;
                bloke.Health = 30;
                bloke.HitProbability = 0.5f;
                break;
            case BlokeType.SystemAdmin: //Good hit prob ok other stats
                bloke.Damage = 7;
                bloke.Health = 20;
                bloke.HitProbability = 0.8f;
                break;
            case BlokeType.Network: //Middle stats
                bloke.Damage = 7;
                bloke.Health = 25;
                bloke.HitProbability = 0.5f;
                break;
            case BlokeType.Developer: //Glass cannon
                bloke.Damage = 15;
                bloke.Health = 12;
                bloke.HitProbability = 0.7f;
                break;
            case BlokeType.DoItAll: //Rare and epic
                bloke.Damage = 15;
                bloke.Health = 30;
                bloke.HitProbability = 0.9f;
                break;
        }

        return bloke;
    }

    private static BlokeType GetBlokeType( int locationX, int locationY )
    {
        var type = BlokeType.Manager;

        Random r = new Random();
        var seed = r.Next( 0, 100 );

        if ( seed == 0 )
        {
            type = BlokeType.DoItAll;
        }
        if ( seed < 50 )
        {
            if ( locationX < 5 )
            {
                if ( locationY < 5 )
                {
                    type = BlokeType.HelpDesk;
                }
                else
                {
                    type = BlokeType.SystemAdmin;
                }
            }
            else
            {
                if ( locationY < 5 )
                {
                    type = BlokeType.Network;
                }
                else
                {
                    type = BlokeType.Developer;
                }
            }
        }
        return type;
    }
}
