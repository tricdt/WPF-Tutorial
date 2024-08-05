﻿using Reservoom.Models;

namespace Reservoom.Stores
{
    public class HotelStore
    {
        private readonly Hotel _hotel;
        private readonly Lazy<Task> _initializeLazy;
        private readonly List<Reservation> _reservations;

        public IEnumerable<Reservation> Reservations => _reservations;

        public event Action<Reservation> ReservationMade;
        public HotelStore(Hotel hotel)
        {
            _hotel = hotel;
            _initializeLazy = new Lazy<Task>(Initialize);
            _reservations = new List<Reservation>();
        }
        public async Task Load()
        {
            await _initializeLazy.Value;
        }
        public async Task MakeReservation(Reservation reservation)
        {
            await _hotel.MakeReservation(reservation);

            _reservations.Add(reservation);

            OnReservationMade(reservation);
        }
        private void OnReservationMade(Reservation reservation)
        {
            ReservationMade?.Invoke(reservation);
        }
        private async Task Initialize()
        {
            IEnumerable<Reservation> reservations = await _hotel.GetAllReservations();
            _reservations.Clear();
            _reservations.AddRange(reservations);

        }
    }
}
