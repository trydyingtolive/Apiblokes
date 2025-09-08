using Apiblokes.Data;
using Apiblokes.Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "You are in a vast field. You see wild Apiblokes scattering before you.");

app.MapPost( "/", () =>
{
    var player = new Player();
    var context = new DataContext();
    context.Players.Add(player);
    context.SaveChanges();
    return player.Id;
} );

app.Run();
