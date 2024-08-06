using Reservoom.Stores;
using Reservoom.ViewModels;
namespace Reservoom.Commands
{
    public class LoadReservationsCommand : AsyncCommandBase
    {
        private readonly ReservationListingViewModel _viewModel;
        private readonly HotelStore _hotelStore;

        public LoadReservationsCommand(ReservationListingViewModel viewModel, HotelStore hotelStore)
        {
            _viewModel = viewModel;
            _hotelStore = hotelStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _viewModel.ErrorMessage = string.Empty;
            _viewModel.IsLoading = true;
            try
            {
                await _hotelStore.Load();
                _viewModel.UpdateReservations(_hotelStore.Reservations);
            }
            catch (Exception ex)
            {
                _viewModel.ErrorMessage = "Failed to load reservations.";
            }
            _viewModel.IsLoading = false;
        }
    }
}
