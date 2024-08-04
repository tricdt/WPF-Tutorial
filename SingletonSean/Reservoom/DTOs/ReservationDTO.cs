using System.ComponentModel.DataAnnotations;

namespace Reservoom.DTOs
{
    public class ReservationDTO
    {
        [Key]
        public Guid Id { get; set; }
        public int FloorNumber { get; set; }
        public int RoomNumber { get; set; }
        public string Username { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
    }
}
