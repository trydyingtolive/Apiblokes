namespace Apiblokes.Game.Data;

public interface IDataContextFactory
{
    IDataContext CreateContext();
}
