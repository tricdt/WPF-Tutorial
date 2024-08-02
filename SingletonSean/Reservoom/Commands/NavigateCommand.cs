using Reservoom.Stores;
using Reservoom.ViewModels;
namespace Reservoom.Commands
{
    public class NavigateCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        public NavigateCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new MakeReservationViewModel(new Models.Hotel("Hihi"));
        }
    }
}
