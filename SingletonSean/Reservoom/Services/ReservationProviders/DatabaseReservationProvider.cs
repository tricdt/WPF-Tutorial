using Microsoft.EntityFrameworkCore;
using Reservoom.DbContexts;
using Reservoom.DTOs;
using Reservoom.Models;

namespace Reservoom.Services.ReservationProviders
{
    public class DatabaseReservationProvider : IReservationProvider
    {
        private readonly ReservoomDbContextFactory _dbContextFactory;

        public DatabaseReservationProvider(ReservoomDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            using (ReservoomDbContext context = _dbContextFactory.CreateDbContext())
            {
                IEnumerable<ReservationDTO> reservationDTOs = await context.Reservations.ToListAsync();
                await Task.Delay(2000);
                return reservationDTOs.Select(r => ToReservation(r));
            }
        }

        private Reservation ToReservation(ReservationDTO r)
        {
            return new Reservation(new RoomID(r.FloorNumber, r.RoomNumber), r.Username, r.StartTime, r.EndTime);
        }
    }
}
