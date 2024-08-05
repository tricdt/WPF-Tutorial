using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Reservoom.DbContexts
{
    public class ReservoomDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ReservoomDbContext>
    {
        public ReservoomDbContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer("Data Source=ODEGAARD\\SQLEXPRESS;Initial Catalog=Reservoom;Integrated Security=True;Trust Server Certificate=True").Options;
            return new ReservoomDbContext(options);
        }
    }
}
