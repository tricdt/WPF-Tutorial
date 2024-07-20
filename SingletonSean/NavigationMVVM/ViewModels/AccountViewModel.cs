using NavigationMVVM.Commands;
using NavigationMVVM.Stores;
using System.Windows.Input;
namespace NavigationMVVM.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public string Name => "SingletonSean";

        public ICommand NavigateHomeCommand { get; }
        public AccountViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            NavigateHomeCommand = new NavigationHomeCommand(_navigationStore);
        }
    }
}
