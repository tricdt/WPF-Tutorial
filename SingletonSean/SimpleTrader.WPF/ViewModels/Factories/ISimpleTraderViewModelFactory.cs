using SimpleTrader.WPF.State.Navigators;

namespace SimpleTrader.WPF.ViewModels.Factories
{
    public interface ISimpleTraderViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
    public interface ISimpleTraderViewModelFactory<T> where T : ViewModelBase
    {
        T CreateViewModel();
    }
}
