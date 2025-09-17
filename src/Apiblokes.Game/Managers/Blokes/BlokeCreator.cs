using Apiblokes.Game.Helpers;
using Apiblokes.Game.Model;

namespace Apiblokes.Game.Managers.Blokes;

public static class BlokeCreator
{
    public static Bloke CreateStarterBloke( Guid playerId )
    {
        var bloke = CreateBlokeFromType( BlokeType.Manager );
        
        bloke.PlayerId = playerId;
        
        return bloke;
    }

    public static Bloke CreateBloke( int locationX, int locationY )
    {
        var blokeType = GetBlokeTypeFromLocation( locationX, locationY );

        var bloke = CreateBlokeFromType( blokeType );

        bloke.X = locationX;
        bloke.Y = locationY;

        return bloke;
    }

    private static Bloke CreateBlokeFromType( BlokeType blokeType )
    {
        var bloke = new Bloke
        {
            Name = GenerateBlokeName(),
            Type = blokeType,
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

    private static BlokeType GetBlokeTypeFromLocation( int locationX, int locationY )
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

    private static string GenerateBlokeName()
    {
        Random r = new Random();

        var firstName = NamesList.FirstNames[r.Next( 0, NamesList.FirstNames.Length )].Capitalize();
        var lastName = NamesList.LastNames[r.Next( 0, NamesList.LastNames.Length )].Capitalize();

        return firstName + " " + lastName;
    }
}
