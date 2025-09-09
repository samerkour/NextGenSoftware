using System.Globalization;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BuildingBlocks.Persistence.EfCore.SqlServer;

public abstract class DbContextDesignFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    private readonly string _moduleName;
    private readonly string _SqlServerOptionsection;

    protected DbContextDesignFactoryBase(string moduleName, string SqlServerOptionsection)
    {
        _moduleName = moduleName.ToLower(CultureInfo.InvariantCulture);
        _SqlServerOptionsection = SqlServerOptionsection;
    }

    public TDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"BaseDirectory: {AppContext.BaseDirectory}");
        Console.WriteLine($"SqlServer Option Section: {_SqlServerOptionsection}");

        var configuration = ConfigurationHelper.GetConfiguration(_moduleName, AppContext.BaseDirectory);
        var options = configuration.GetOptions<SqlServerOptions>(_SqlServerOptionsection);

        if (string.IsNullOrWhiteSpace(options?.ConnectionString))
        {
            throw new InvalidOperationException("Could not find a connection string.");
        }

        Console.WriteLine($"Connection String: {options.ConnectionString}");

        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>()
            .UseSqlServer(
                options.ConnectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                }
            );

        Console.WriteLine(options.ConnectionString);
        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options);
    }
}
