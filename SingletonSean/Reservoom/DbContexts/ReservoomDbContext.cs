using Microsoft.EntityFrameworkCore;
using Reservoom.DTOs;
namespace Reservoom.DbContexts
{
    public class ReservoomDbContext : DbContext
    {
        public DbSet<ReservationDTO> Reservations { get; set; }
    }
}
