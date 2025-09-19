using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Telnet.Commanding;

public class Command
{
    public string[] CommandStrings { get; set; } = [];
    public string Description { get; set; } = string.Empty;

    public Func<string, string, PlayerManager, Task<string[]>>? CommandAction { get; set; }
}