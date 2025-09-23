using Apiblokes.Game.Helpers;

namespace Apiblokes.Game.Model;
public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "Trainer";

    public string PassKey { get; set; } = Utilities.RandomString( 6 );

    public int Money { get; set; } = 0;

    public int Level2Catchers { get; set; } = 0;

    public int Level3Catchers { get; set; } = 0;

    public int X { get; set; } = 5;
    public int Y { get; set; } = 5;
}