using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Reservoom.ViewModels
{
    public class ReservationListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ReservationViewModel> _reservations;
        private readonly Hotel _hotel;
        public IEnumerable<ReservationViewModel> Reservations => _reservations;
        public ICommand MakeReservationCommand { get; }
        public ReservationListingViewModel(Hotel hotel, NavigationService navigationService)
        {
            _hotel = hotel;
            _reservations = new ObservableCollection<ReservationViewModel>();
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(1, 2), "SingletonSean", DateTime.MinValue, DateTime.MaxValue)));
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(3, 2), "Joe", DateTime.MinValue, DateTime.MaxValue)));
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(2, 4), "Mary", DateTime.MinValue, DateTime.MaxValue)));
            MakeReservationCommand = new NavigateCommand(navigationService);
            UpdateReservations();
        }
        private void UpdateReservations()
        {
            _reservations.Clear();
            foreach (Reservation reservation in _hotel.GetAllReservations())
            {
                ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
                _reservations.Add(reservationViewModel);
            }
        }
    }
}
