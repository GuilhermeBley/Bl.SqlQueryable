using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bl.SqlQueryable.Dapper.Tests;

public class DefaultHost
{
    private IServiceProvider _serviceProvider;
    public IServiceProvider Provider => _serviceProvider;

    public DefaultHost()
    {
        _serviceProvider = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(ConfigureConfiguration)
            .ConfigureServices(ConfigureServices)
            .Build().Services;
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
    {
        serviceCollection.Add<System.Data.IDbConnection>(
            (provider) => new MySqlConnection);
    }

    private void ConfigureConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
    {
        
    }

    public IServiceProvider CreateScope()
        => Provider.CreateScope().ServiceProvider;
}