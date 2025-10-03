using System.Net.Sockets;
using System.Text;
using Apiblokes.Game.Helpers;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Telnet.Commanding;

namespace Apiblokes.Telnet;
public class TelnetClient
{
    private readonly TcpClient _tcpClient;
    private readonly NetworkStream _stream;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;
    private readonly TelnetServer _server;
    private readonly Commands _commands;
    private string? _playerName;
    private string? _playerPassKey;
    private string _lastMessage = string.Empty;

    private readonly IPlayerManagerBuilder _playerManagerBuilder;

    public bool IsConnected => _tcpClient?.Connected ?? false;

    public TelnetClient( TcpClient tcpClient,
        TelnetServer server,
        IPlayerManagerBuilder playerManagerBuilder,
        Commands commands )
    {
        _tcpClient = tcpClient;
        _server = server;
        _stream = tcpClient.GetStream();
        _reader = new StreamReader( _stream, Encoding.ASCII );
        _writer = new StreamWriter( _stream, Encoding.ASCII ) { AutoFlush = true };
        _playerManagerBuilder = playerManagerBuilder;
        _commands = commands;
    }

    public async Task HandleAsync()
    {
        try
        {
            // Send welcome message
            await _writer.WriteLineAsync( "Welcome to Apiblokes!!" );

            await GetOrCreateUser();

            // Main command loop
            while ( IsConnected )
            {
                await _writer.WriteAsync( $"{_playerName}> " );
                var output = await ReadInput();
                if ( !string.IsNullOrEmpty( output ) )
                {
                    await ProcessCommand( output );
                }
            }
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"Client error: {ex.Message}" );
        }
        finally
        {
            Disconnect();
        }
    }

    private async Task<string?> ReadInput()
    {
        var message = string.Empty;
        var c = new char[64];

        var previousBuffer = new char[64];
        var repeatCount = 0;

        var isFirstRun = true;

        while ( IsConnected )
        {

            var len = await _reader.ReadAsync( c );


            //If we loose connection it will just run in a loop. (Weird.)
            if ( Enumerable.SequenceEqual( c, previousBuffer ) )
            {
                repeatCount++;
                if ( repeatCount >= 64 )
                {
                    Disconnect();
                }
            }
            else
            {
                repeatCount = 0;
            }

            Array.Copy( c, previousBuffer, 64 );


            //Some clients love to send command codes at the beginning.
            //I don't want to handle them, so we will just ignore :D
            //Maybe later fix
            if ( isFirstRun && c[0] == '?' )
            {
                continue;
            }
            isFirstRun = false;

            for ( var i = 0; i < len; i++ )
            {
                if ( c[i] == '\b' || c[i] == 127 )
                {
                    if ( message.Length > 0 )
                    {
                        message = message.Substring( 0, message.Length - 1 );
                        await _writer.WriteAsync( " " + c[i] );
                    }
                }
                else if ( c[i] == '\n' )
                {
                    return message.Trim();
                }
                else if ( IsValidCharacter( c[i] ) )
                {
                    message += c[i];
                }

            }
        }

        return default;
    }

    private bool IsValidCharacter( char ch )
    {
        return char.IsAscii( ch );
    }

    private async Task GetOrCreateUser()
    {
        await _writer.WriteLineAsync( "If you know your login token enter it here." );
        await _writer.WriteLineAsync( "Otherwise type 'new'." );
        await _writer.WriteAsync( "> " );

        var output = await ReadInput();

        if ( output.ToLower().Contains( "new" ) )
        {
            await CreateUser();
            return;
        }

        var playerManager = await _playerManagerBuilder.FromKeyAsync( output );

        if ( playerManager == null )
        {
            await _writer.WriteLineAsync( "Player could not be found." );
            await _writer.WriteLineAsync( "" );
            await GetOrCreateUser();
            return;
        }

        playerManager.OnGlobalNotification += PlayerManager_OnGlobalNotification;

        _playerName = playerManager.Name;
        _playerPassKey = playerManager.PassKey;

        await _writer.WriteLineAsync( $"Welcome back {_playerName}" );
        await _writer.WriteLineAsync( "" );
        await _writer.WriteLineAsync( await playerManager.GetStatusAsync() );
    }

    private void PlayerManager_OnGlobalNotification( object? sender, EventArgs e )
    {
        if ( e is GlobalNotificationArgs args )
        {
            var globalMessage = e as GlobalNotificationArgs;
            if ( globalMessage?.Message != null )
            {
                _server.BroadcastMessage( globalMessage.Message );
            }
        }
    }

    private async Task CreateUser()
    {
        await _writer.WriteLineAsync( "" );
        await _writer.WriteLineAsync( "Please enter your player name:" );

        var playerName = ( await ReadInput() )?.Trim().Truncate( 10 );

        if ( string.IsNullOrEmpty( playerName ) )
        {
            await CreateUser();
            return;
        }

        await _writer.WriteLineAsync( "" );
        await _writer.WriteLineAsync( $"Confirm you want you player to be named: {playerName} (y/N)" );

        var response = ( await ReadInput() ) ?? "";

        if ( !response.Trim().StartsWith( "y", StringComparison.CurrentCultureIgnoreCase ) )
        {
            await CreateUser();
            return;
        }

        var playerManager = await _playerManagerBuilder.FromNewPlayer( playerName );
        _playerPassKey = playerManager.PassKey;
        _playerName = playerName;

        await _writer.WriteLineAsync( "" );
        await _writer.WriteLineAsync( $"Welcome {_playerName}" );
        await _writer.WriteLineAsync( $"Your Pass Key is {_playerPassKey}" );

        await _writer.WriteLineAsync( "Please save it some place safe, you will not be able to access your player without it." );

        await _writer.WriteLineAsync( "" );

        await _writer.WriteLineAsync( await playerManager.GetStatusAsync() );

        playerManager.OnGlobalNotification += PlayerManager_OnGlobalNotification;
    }

    private async Task ProcessCommand( string message )
    {
        //Repeat Last Task
        if ( message.Trim().ToLower() == "r" )
        {
            await ProcessCommand( _lastMessage );
            return;
        }

        _lastMessage = message;

        (string commandText, string argumentText) = ParseMessage( message );

        if ( string.IsNullOrEmpty( commandText ) )
        {
            await _writer.WriteLineAsync( "Command not found. Type help for available commands" );
            return;
        }

        //Exit
        if ( commandText == "quit" || commandText == "exit" )
        {
            await _writer.WriteLineAsync( "Goodbye!" );
            Disconnect();
            return;
        }

        //Help
        if ( commandText == "h" || commandText == "help" )
        {
            await _writer.WriteLineAsync( _commands.HelpCommand() );
            return;
        }

        foreach ( var command in _commands.ActiveCommands )
        {
            if ( command.CommandStrings.Contains( commandText ) )
            {
                if ( command.CommandAction != null )
                {
                    var playerManager = await _playerManagerBuilder.FromKeyAsync( _playerPassKey ?? "" );

                    if ( playerManager == null )
                    {
                        return;
                    }

                    var commandResponses = await command.CommandAction( commandText, argumentText, playerManager );

                    foreach ( var response in commandResponses )
                    {
                        await _writer.WriteLineAsync( response );
                        await Task.Delay( 500 );
                    }
                }
                return;
            }
        }

        await _writer.WriteLineAsync( "Command not found. Type help for available commands" );
    }

    /// <summary>
    /// Takes a message from the user and splits it into the base command and the argument
    /// </summary>
    /// <param name="message">Message from the user</param>
    /// <returns></returns>
    private (string command, string argument) ParseMessage( string message )
    {
        message = message.Trim();

        if ( string.IsNullOrEmpty( message ) )
        {
            return (string.Empty, string.Empty);
        }
        var firstSpaceIndex = message.IndexOf( " " );

        var firstString = message;
        var secondString = string.Empty;

        if ( firstSpaceIndex > 0 )
        {
            firstString = message.Substring( 0, firstSpaceIndex );
            secondString = message.Substring( firstSpaceIndex + 1 );
        }


        return (firstString.ToLower().Trim(), secondString);
    }

    public void SendMessage( string message )
    {
        try
        {
            if ( IsConnected )
            {
                _writer.WriteLine( message );
            }
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"Error sending message: {ex.Message}" );
        }
    }

    public void Disconnect()
    {
        try
        {
            _reader?.Close();
            _writer?.Close();
            _stream?.Close();
            _tcpClient?.Close();

            _server.RemoveClient( this );
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"Error during disconnect: {ex.Message}" );
        }
    }
}
