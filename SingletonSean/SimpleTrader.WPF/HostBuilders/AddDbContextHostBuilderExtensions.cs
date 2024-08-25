using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrader.EntityFramework;

namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            string connectionString = "Data Source=ODEGAARD\\SQLEXPRESS;Initial Catalog=SimpleTraderDB;Integrated Security=True;Trust Server Certificate=True";
            host.ConfigureServices((context, services) =>
            {
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlServer(connectionString);
                services.AddDbContext<SimpleTraderDbContext>(configureDbContext);
                services.AddSingleton<SimpleTraderDbContextFactory>(new SimpleTraderDbContextFactory(configureDbContext));
            });
            return host;
        }
    }
}
