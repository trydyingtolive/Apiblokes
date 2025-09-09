namespace Apiblokes.Game.Model;
public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int X { get; set; } = 5;
    public int Y { get; set; } = 5;

    public void Move( string direction )
    {
        var simpleDir = direction.ToLower().FirstOrDefault();

        switch ( simpleDir )
        {
            case 'n':
                Y += 1;
                break;
            case 's':
                Y -= 1;
                break;
            case 'w':
                X -= 1;
                break;
            case 'e':
                Y += 1;
                break;
            default:
                break;
        }
    }
}

