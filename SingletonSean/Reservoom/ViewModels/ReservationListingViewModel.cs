using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Reservoom.ViewModels
{
    public class ReservationListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ReservationViewModel> _reservations;
        public IEnumerable<ReservationViewModel> Reservations => _reservations;
        public ICommand MakeReservationCommand { get; }
        public ReservationListingViewModel(NavigationStore navigationStore)
        {
            _reservations = new ObservableCollection<ReservationViewModel>();
            _reservations.Add(new ReservationViewModel(new Reservation(new RoomID(1, 2), "SingletonSean", DateTime.MinValue, DateTime.MaxValue)));
            _reservations.Add(new ReservationViewModel(new Reservation(new RoomID(3, 2), "Joe", DateTime.MinValue, DateTime.MaxValue)));
            _reservations.Add(new ReservationViewModel(new Reservation(new RoomID(2, 4), "Mary", DateTime.MinValue, DateTime.MaxValue)));
            MakeReservationCommand = new NavigateCommand(navigationStore);
        }
    }
}
