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
        public ICommand LoadReservationCommand { get; }
        public ReservationListingViewModel(Hotel hotel, NavigationService navigationService)
        {
            _hotel = hotel;
            _reservations = new ObservableCollection<ReservationViewModel>();
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(1, 2), "SingletonSean", DateTime.MinValue, DateTime.MaxValue)));
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(3, 2), "Joe", DateTime.MinValue, DateTime.MaxValue)));
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(2, 4), "Mary", DateTime.MinValue, DateTime.MaxValue)));
            MakeReservationCommand = new NavigateCommand(navigationService);
            LoadReservationCommand = new LoadReservationsCommand(this, hotel);
            //UpdateReservations();
        }
        public void UpdateReservations(IEnumerable<Reservation> reservations)
        {
            _reservations.Clear();
            foreach (Reservation reservation in reservations)
            {
                ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
                _reservations.Add(reservationViewModel);
            }
        }
        public static ReservationListingViewModel LoadViewModel(Hotel hotel, NavigationService navigationService)
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(hotel, navigationService);
            viewModel.LoadReservationCommand.Execute(null);
            return viewModel;
        }
    }
}
