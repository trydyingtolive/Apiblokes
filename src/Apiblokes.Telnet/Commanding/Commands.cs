using Apiblokes.Game.Managers.Players;

namespace Apiblokes.Telnet.Commanding;

public class Commands
{
    public List<Command> ActiveCommands { get; set; } = new List<Command>
    {
        new Command
        {
            CommandStrings = ["help","h"],
            Description = "Gets available commands"
        },
        new Command
        {
            CommandStrings = ["look", "l"],
            Description = "Look at the world you are in.",
            CommandAction = async (command, arguments,playerManager) => {  return  [await playerManager.GetStatusAsync()];  }
        },
        new Command
        {
            CommandStrings = ["inventory", "i"],
            Description = "Dig through your pockets and report on contents",
            CommandAction = async (command, arguments,playerManager) => {  return [await playerManager.GetInventoryAsync()];  }
        },
        new Command
        {
            CommandStrings = ["move", "m", "n", "s", "e", "w"],
            Description = "Moves your player north, south, east, or west. Can be shortened to just 'n' 's' 'e' or 'w'",
            CommandAction = MoveCommand
        },
        new Command
        {
            CommandStrings = ["attack", "a"],
            Description = "Attacks bloke with one from your inventory. Ex: 'attack Stew Martin with Azana Yoder'",
            CommandAction = async (command, arguments,playerManager) => {  return await playerManager.AttemptAttackAsync(arguments); }
        },
        new Command
        {
            CommandStrings = ["r"],
            Description = "Repeats the last action. Useful for battles.",
            CommandAction = async( command, arguments, playerManager) =>{  return []; }
        },
        new Command
        {
            CommandStrings = ["use"],
            Description ="Uses an item or building"
        }
    };

    private static async Task<string[]> MoveCommand( string command, string arguments, PlayerManager playerManager )
    {
        switch ( command )
        {
            case "move":
            case "m":
                await playerManager
            .MovePlayerAsync( arguments );
                break;
            case "n":
            case "s":
            case "e":
            case "w":
                await playerManager
            .MovePlayerAsync( command );
                break;
            default:
                break;
        }
        return [await playerManager.GetStatusAsync()];
    }

    public string HelpCommand()
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
