namespace BuildingBlocks.Persistence.EfCore.SqlServer;

public class SqlServerOptions
{
    public string ConnectionString { get; set; } = null!;
    public bool UseInMemory { get; set; }
}
