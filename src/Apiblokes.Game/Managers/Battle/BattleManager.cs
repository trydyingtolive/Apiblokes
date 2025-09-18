using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Apiblokes.Game.Managers.Blokes;

namespace Apiblokes.Game.Managers.Battle;

public class BattleManager
{
    private readonly BlokeManager attackingBloke;
    private readonly BlokeManager defendingBloke;

    public async static Task<(BattleManager?, string)> SetupBattleAsync( IBlokeManagerBuilder blokeManagerBuilder, BattleRequestOptions options )
    {
        var parts = options.RequestText.ToLower().Split( "with" );
        if ( parts.Length < 2 )
        {
            return (null, "Battle failed: Could not find two names in attack text");
        }

        var attackingBloke = options.AvailablePlayerBlokes.FirstOrDefault( b => b.Name.ToLower().Contains( parts[1].Trim() ) );
        if ( attackingBloke == null )
        {
            return (null, "Battle failed: Could not find player Apibloke");
        }

        var localBlokes = await blokeManagerBuilder.AllFromWorldLocation( options.X, options.Y );
        var defendingBloke = localBlokes.FirstOrDefault( b => b.Name.ToLower().Contains( parts[0].Trim() ) );
        if ( defendingBloke == null )
        {
            return (null, $"Battle failed: Could not find Apibloke in location: {options.X}:{options.Y}");
        }

        return (new BattleManager( attackingBloke, defendingBloke ), $"The battle begins {attackingBloke.Name} vs {defendingBloke.Name}");
    }


    private BattleManager( BlokeManager attackingBloke, BlokeManager defendingBloke )
    {
        this.attackingBloke = attackingBloke;
        this.defendingBloke = defendingBloke;
    }

    public async Task<List<string>> ProcessBattleAsync()
    {
        var output = new List<string>();

        if ( attackingBloke.Health <= 0 )
        {
            output.Add( $"{attackingBloke.Name}'s ambition far exceeds their heath. With no health remaining the only thing left to do is fall over in a crumpled heap." );
            return output;
        }

        var r = new Random();
        var result = r.NextDouble();

        if ( result <= attackingBloke.HitProbability )
        {
            await defendingBloke.TakeDamageAsync( attackingBloke.Damage );
            output.Add( $"{attackingBloke.Name} does {attackingBloke.Damage} points of damage. {defendingBloke.Name} has {defendingBloke.Health} remaining." );
        }
        else
        {
            output.Add( "The attack failed" );
        }

        if ( defendingBloke.Health <= 0 )
        {
            output.Add( $"{defendingBloke.Name} does not fight back" );
            return output;
        }

        result = r.NextDouble();
        if ( result <= defendingBloke.HitProbability )
        {
            await attackingBloke.TakeDamageAsync( defendingBloke.Damage );
            output.Add( $"{defendingBloke.Name} does {defendingBloke.Damage} points of damage. {attackingBloke.Name} has {attackingBloke.Health} remaining." );
        }
        else
        {
            output.Add( "The counter attack failed" );
        }

        return output;
    }

}

public class BattleRequestOptions
{
    public string RequestText { get; set; } = string.Empty;

    public List<BlokeManager> AvailablePlayerBlokes { get; set; } = new();

    public int X { get; set; }
    public int Y { get; set; }
}