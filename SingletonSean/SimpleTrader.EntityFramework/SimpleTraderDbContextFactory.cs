using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleTrader.EntityFramework
{
    public class SimpleTraderDbContextFactory : IDesignTimeDbContextFactory<SimpleTraderDbContext>
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;
        public SimpleTraderDbContext CreateDbContext(string[] args = null)
        {

            DbContextOptionsBuilder<SimpleTraderDbContext> options = new DbContextOptionsBuilder<SimpleTraderDbContext>();
            _configureDbContext(options);
            return new SimpleTraderDbContext(options.Options);
        }
        public SimpleTraderDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }
    }
}
