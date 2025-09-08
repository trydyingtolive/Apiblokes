namespace Apiblokes.Model;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int X { get; set; } = 5;
    public int Y { get; set; } = 5;
}
