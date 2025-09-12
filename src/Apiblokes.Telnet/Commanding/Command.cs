using Apiblokes.Game.Managers;

namespace Apiblokes.Telnet.Commanding;

internal class Command
{
    public string[] CommandStrings { get; set; } = [];
    public string Description { get; set; } = string.Empty;

    public Func <string, PlayerManager, Task<string> >? CommandAction { get; set; }
}