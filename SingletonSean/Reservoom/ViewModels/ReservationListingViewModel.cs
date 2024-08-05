using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Reservoom.ViewModels
{
    public class ReservationListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ReservationViewModel> _reservations;
        private readonly HotelStore _hotelStore;
        public IEnumerable<ReservationViewModel> Reservations => _reservations;
        public ICommand MakeReservationCommand { get; }
        public ICommand LoadReservationCommand { get; }
        public ReservationListingViewModel(HotelStore hotelStore, NavigationService navigationService)
        {
            _hotelStore = hotelStore;
            _reservations = new ObservableCollection<ReservationViewModel>();
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(1, 2), "SingletonSean", DateTime.MinValue, DateTime.MaxValue)));
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(3, 2), "Joe", DateTime.MinValue, DateTime.MaxValue)));
            //_reservations.Add(new ReservationViewModel(new Reservation(new RoomID(2, 4), "Mary", DateTime.MinValue, DateTime.MaxValue)));
            MakeReservationCommand = new NavigateCommand(navigationService);
            LoadReservationCommand = new LoadReservationsCommand(this, hotelStore);
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
        public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, NavigationService navigationService)
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(hotelStore, navigationService);
            viewModel.LoadReservationCommand.Execute(null);
            return viewModel;
        }
    }
}
