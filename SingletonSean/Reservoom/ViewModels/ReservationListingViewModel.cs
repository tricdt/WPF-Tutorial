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
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));

                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public ICommand MakeReservationCommand { get; }
        public ICommand LoadReservationCommand { get; }
        public ReservationListingViewModel(HotelStore hotelStore, NavigationService navigationService)
        {
            _hotelStore = hotelStore;
            _reservations = new ObservableCollection<ReservationViewModel>();
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
