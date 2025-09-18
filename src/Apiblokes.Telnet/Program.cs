using System;
using Apiblokes.Game.Data;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Players;
using Apiblokes.Telnet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder( args );

var db = new DataContext();
db.Database.EnsureCreated();

builder.Services.AddSingleton<IDataContextFactory, DataContextFactory>();
builder.Services.AddSingleton<IPlayerManagerBuilder, PlayerManagerBuilder>();
builder.Services.AddSingleton<IBlokeManagerBuilder, BlokeManagerBuilder>();

builder.Services.AddHostedService<WorldPopulationManager>();

builder.Services.AddHostedService<TelnetServer>();

using IHost host = builder.Build();


host.Run();