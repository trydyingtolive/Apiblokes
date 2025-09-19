namespace Apiblokes.Game.Model;

public class Bloke
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public BlokeType Type { get; set; }

    public float HitProbability { get; set; } = .5f;

    public int Damage { get; set; } = 1;

    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;

    public int X { get; set; }
    public int Y { get; set; }

    public Guid? PlayerId { get; set; }

    public virtual Player? Player { get; set; }

    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
}

public enum BlokeType
{
    Manager = 0,
    HelpDesk = 1,
    Network = 2,
    SystemAdmin = 3,
    Developer = 4,
    DoItAll = 5,
}