using Apiblokes.Game.Data;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Items;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Telnet;
using Apiblokes.Telnet.Commanding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder( args );

var db = new DataContext();
db.Database.EnsureCreated();

builder.Services.AddSingleton<IDataContextFactory, DataContextFactory>();
builder.Services.AddSingleton<IPlayerManagerBuilder, PlayerManagerBuilder>();
builder.Services.AddSingleton<IBlokeManagerBuilder, BlokeManagerBuilder>();
builder.Services.AddSingleton<IUsableItemFactory, UsableItemFactory>();

builder.Services.AddSingleton<Commands>();

builder.Services.AddHostedService<WorldPopulationManager>();
builder.Services.AddHostedService<TelnetServer>();

using IHost host = builder.Build();


host.Run();
