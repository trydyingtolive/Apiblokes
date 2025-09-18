using System;
using Apiblokes.Game.Data;
using Apiblokes.Game.Managers.Blokes;
using Apiblokes.Game.Managers.Game;
using Apiblokes.Telnet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder( args );

var db = new DataContext();
db.Database.EnsureCreated();

builder.Services.AddScoped<IDataContextFactory, DataContextFactory>();
builder.Services.AddScoped<IGameManager, GameManager>();
builder.Services.AddScoped<IBlokeManagerBuilder, BlokeManagerBuilder>();

builder.Services.AddHostedService<WorldPopulationManager>();

builder.Services.AddHostedService<TelnetServer>();

using IHost host = builder.Build();


host.Run();