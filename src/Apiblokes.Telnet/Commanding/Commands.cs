namespace Apiblokes.Telnet.Commanding;

internal static class Commands
{
    public static List<Command> ActiveCommands { get; set; } = new List<Command>
    {
        new Command
        {
            CommandStrings = ["help","h"],
            Description = "Gets available commands",
            CommandAction = (text, playerManager) => { return Task.FromResult( HelpCommand()); }
        },
        new Command
        {
            CommandStrings = ["echo"],
            Description = "Echoes back the text you write",
            CommandAction = (text,playerManager) => { return Task.FromResult("Echoing: " + text); }
        },
        new Command
        {
            CommandStrings = ["move", "m"],
            Description = "Moves your player north, south, east, or west. Ex: move w",
            CommandAction = async (text,playerManager) => { await playerManager.MovePlayerAsync(text); return playerManager.GetStatus(); }
        }
    };

    private static string HelpCommand()
    {
        var output = string.Empty;
        foreach ( var command in ActiveCommands )
        {
            var primaryKey = command.CommandStrings.FirstOrDefault();
            if ( string.IsNullOrEmpty( primaryKey ) )
            {
                continue;
            }
            output += $"{primaryKey}: {command.Description}\r\n";
        }

        return output;
    }
}
