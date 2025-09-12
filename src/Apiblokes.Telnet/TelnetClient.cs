using System.Net.Sockets;
using System.Text;
using Apiblokes.Game.Managers;
using Apiblokes.Telnet.Commanding;

namespace Apiblokes.Telnet;
public class TelnetClient
{
    private readonly TcpClient _tcpClient;
    private readonly NetworkStream _stream;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;
    private readonly TelnetServer _server;
    private string? _playerName;
    private string? _playerId;

    private IGameManager _gameManager;

    public bool IsConnected => _tcpClient?.Connected ?? false;

    public TelnetClient( TcpClient tcpClient, TelnetServer server, Game.Managers.IGameManager gameManager )
    {
        _tcpClient = tcpClient;
        _server = server;
        _stream = tcpClient.GetStream();
        _reader = new StreamReader( _stream, Encoding.ASCII );
        _writer = new StreamWriter( _stream, Encoding.ASCII ) { AutoFlush = true };
        _gameManager = gameManager;
    }

    public async Task HandleAsync()
    {
        try
        {
            // Send welcome message
            await _writer.WriteLineAsync( "Welcome to Apiblokes!!" );

            await GetOrCreateUser();

            await _writer.WriteLineAsync( $"Hello {_playerName}! You are now connected." );
            await _writer.WriteLineAsync( "Type 'help' for available commands or 'quit' to exit." );

            // Notify other clients
            _server.BroadcastMessage( $"*** {_playerName} joined the server ***", this );



            // Main command loop
            while ( IsConnected )
            {
                await _writer.WriteAsync( $"{_playerName}> " );
                var output = await ReadInput();
                await ProcessCommand( output );

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

    private async Task<string> ReadInput()
    {
        var message = string.Empty;
        var c = new char[64];

        while ( IsConnected )
        {

            var len = await _reader.ReadAsync( c );

            if ( len == 1 )
            {
                if ( c[0] == '\b' )
                {
                    if ( message.Length > 0 )
                    {
                        message = message.Substring( 0, message.Length - 1 );
                        await _writer.WriteAsync( " \b" );
                    }
                }
                else
                {
                    message += c[0];
                }
            }

            if ( c[0] == '\n' || c[1] == '\n' ) //user pressed enter
            {
                break;
            }
        }

        return message;
    }

    private async Task GetOrCreateUser()
    {
        await _writer.WriteLineAsync( "If you know your login token enter it here." );
        await _writer.WriteLineAsync( "Otherwise type new." );
        await _writer.WriteAsync( "> " );

        var output = await ReadInput();

        if ( output.ToLower().Contains( "new" ) )
        {
            await CreateUser();
            return;
        }

        var playerManager = await _gameManager.GetPlayerManagerAsync( output );
        _playerId = output;
        _playerName = output;
    }

    private async Task CreateUser()
    {
        var playerManager = await _gameManager.GetPlayerManagerAsync( "" );
        var token = await playerManager.CreateNewPlayerAsync();
        _playerId = token;
        _playerName = token;
    }

    private async Task ProcessCommand( string message )
    {
        (string commandText, string argumentText) = ParseMessage( message );

        if ( string.IsNullOrEmpty( commandText ) )
        {
            await _writer.WriteLineAsync( "Command not found. Type help for available commands" );
            return;
        }

        if ( commandText == "quit" || commandText == "exit" )
        {
            await _writer.WriteLineAsync( "Goodbye!" );
            Disconnect();
            return;
        }

        foreach ( var command in Commands.ActiveCommands )
        {
            if ( command.CommandStrings.Contains( commandText ) )
            {
                if ( command.CommandAction != null )
                {
                    var playerManager = await _gameManager.GetPlayerManagerAsync( _playerId );
                    await _writer.WriteLineAsync( await command.CommandAction( argumentText, playerManager ) );
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


        return (firstString.ToLower(), secondString);
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
            if ( IsConnected )
            {
                _server.BroadcastMessage( $"*** {_playerName} left the server ***", this );
            }

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
