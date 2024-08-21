using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.Models;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
namespace SimpleTrader.WPF.State.Navigators
{
    public class Navigator : ObservableObject, INavigator
    {
        private readonly Subject<ViewModelBase> _subject = new Subject<ViewModelBase>();
        public IObservable<ViewModelBase> WhenNavigationChanged => _subject.AsObservable();
        public event Action CurrentViewModelChanged;
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
                _subject.OnNext(value);
            }
        }

        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public Navigator(ISimpleTraderViewModelFactory viewModelFactory)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);
        }
    }
}
