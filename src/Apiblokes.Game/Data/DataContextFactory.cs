namespace Apiblokes.Game.Data;

public class DataContextFactory : IDataContextFactory
{
    public IDataContext CreateContext() => new DataContext();
}
