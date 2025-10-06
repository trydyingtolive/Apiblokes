namespace Apiblokes.Game.Data;

public class DataContextFactory : IDataContextFactory
{
    public DataContextFactory()
    {
        //Ensures our DB is ready.
        var db = new DataContext();
        db.Database.EnsureCreated();
    }

    public IDataContext CreateContext() => new DataContext();
}
