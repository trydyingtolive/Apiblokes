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

        var localBlokes = await blokeManagerBuilder.AllFromWorldLocationAsync( options.X, options.Y );
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
            if ( defendingBloke.Health > 0 )
            {
                await defendingBloke.TakeDamageAsync( attackingBloke.Damage );
                output.Add( string.Format( BattleFlavor.SuccessfulAttack( attackingBloke.Type, defendingBloke.Type ), attackingBloke.Name, defendingBloke.Name ) );
                output.Add( $"{attackingBloke.Name} does {attackingBloke.Damage} points of damage. {defendingBloke.Name} has {defendingBloke.Health} remaining." );
            }
            else
            {
                await defendingBloke.FireBlokeAsync();

                output.Add( "" );
                output.Add( "**** DEATH ****" );
                output.Add( $"{defendingBloke.Name} was barely hanging on to life, but that didn't stop {attackingBloke.Name} from completing the vicious attack with a killing blow." +
                    $" After the carnage has finished, {attackingBloke.Name} slowly turns to you. Their blood lust eyes meet yours seeking approval. {defendingBloke.Name}'s career is dead." );
                output.Add( "" );
                return output;
            }
        }
        else
        {
            output.Add( string.Format( BattleFlavor.FailedAttack( attackingBloke.Type, defendingBloke.Type ), attackingBloke.Name, defendingBloke.Name ) );
            output.Add( "The attack failed" );
        }

        if ( defendingBloke.Health <= 0 )
        {
            output.Add( $"{defendingBloke.Name} does not fight back" );
            return output;
        }

        output.Add( $"{defendingBloke.Name} counter attacks." );

        result = r.NextDouble();
        if ( result <= defendingBloke.HitProbability )
        {
            await attackingBloke.TakeDamageAsync( defendingBloke.Damage );
            output.Add( string.Format( BattleFlavor.SuccessfulAttack( defendingBloke.Type, attackingBloke.Type ), defendingBloke.Name, attackingBloke.Name ) );
            output.Add( $"{defendingBloke.Name} does {defendingBloke.Damage} points of damage. {attackingBloke.Name} has {attackingBloke.Health} remaining." );
        }
        else
        {
            output.Add( string.Format( BattleFlavor.FailedAttack( defendingBloke.Type, attackingBloke.Type ), defendingBloke.Name, attackingBloke.Name ) );
            output.Add( "The counter attack failed" );
        }

        string? hpResponse = await attackingBloke.AddExperienceAsync( 1 );
        if ( !string.IsNullOrEmpty( hpResponse ) )
        {
            output.Add( hpResponse );
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