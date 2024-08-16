using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleTrader.EntityFramework
{
    public class SimpleTraderDbContextFactory : IDesignTimeDbContextFactory<SimpleTraderDbContext>
    {
        public SimpleTraderDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<SimpleTraderDbContext>();
            options.UseSqlServer("Data Source=ODEGAARD\\SQLEXPRESS;Initial Catalog=SimpleTraderDB;Integrated Security=True;Trust Server Certificate=True");
            return new SimpleTraderDbContext(options.Options);
        }
    }
}
