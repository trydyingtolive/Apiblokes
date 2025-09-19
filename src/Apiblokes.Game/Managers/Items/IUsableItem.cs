namespace Apiblokes.Game.Managers.Items;

public interface IUsableItem
{
    Task<string> UseItemAsync();
}
