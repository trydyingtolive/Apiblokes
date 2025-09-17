using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Apiblokes.Game.Managers.Game;
using Microsoft.Extensions.Hosting;

namespace Apiblokes.Telnet;

public class TelnetServer : BackgroundService
{
    private const int _port = 23;

    private TcpListener _listener;
    private bool _isRunning;
    private readonly List<TelnetClient> _clients;
    private readonly object _clientsLock = new object();

    private readonly IGameManager _gameManager;

    public TelnetServer(IGameManager gameManager)
    {
        _listener = new TcpListener( IPAddress.Any, _port );
        _clients = new List<TelnetClient>();
        _gameManager = gameManager;
    }

    public async Task StartAsync()
    {
        _listener.Start();
        _isRunning = true;

        Console.WriteLine( $"Telnet server started on port {( ( IPEndPoint ) _listener.LocalEndpoint ).Port}" );

        while ( _isRunning )
        {
            try
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                var telnetClient = new TelnetClient( tcpClient, this, _gameManager );

                lock ( _clientsLock )
                {
                    _clients.Add( telnetClient );
                }

                // Handle client in background
                _ = Task.Run( () => telnetClient.HandleAsync() );
            }
            catch ( ObjectDisposedException )
            {
                // Server stopped
                break;
            }
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _listener?.Stop();

        lock ( _clientsLock )
        {
            foreach ( var client in _clients )
            {
                client.Disconnect();
            }
            _clients.Clear();
        }
    }

    public void RemoveClient( TelnetClient client )
    {
        lock ( _clientsLock )
        {
            _clients.Remove( client );
        }
    }

    public void BroadcastMessage( string message, TelnetClient excludeClient = null )
    {
        lock ( _clientsLock )
        {
            foreach ( var client in _clients )
            {
                if ( client != excludeClient && client.IsConnected )
                {
                    client.SendMessage( message );
                }
            }
        }
    }

    protected override async Task ExecuteAsync( CancellationToken stoppingToken )
    {
        await StartAsync();
    }
}