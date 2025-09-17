using System;
using Apiblokes.Game.Data;
using Apiblokes.Game.Managers;
using Apiblokes.Telnet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder( args );

var db = new DataContext();
db.Database.EnsureCreated();

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IGameManager, GameManager>();
builder.Services.AddHostedService<TelnetServer>();

using IHost host = builder.Build();


host.Run();