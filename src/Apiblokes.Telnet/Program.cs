using Apiblokes.Game.Data;
using Apiblokes.Game.Managers;
using Apiblokes.Telnet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder( args );

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IGameManager, GameManager>();
builder.Services.AddHostedService<TelnetServer>();

using IHost host = builder.Build();


host.Run();