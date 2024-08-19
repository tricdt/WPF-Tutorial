namespace SimpleTrader.WPF.ViewModels.Factories
{
    public class HomeViewModelFactory : ISimpleTraderViewModelFactory<HomeViewModel>
    {
        private readonly ISimpleTraderViewModelFactory<MajorIndexListingViewModel> _majorIndexListingViewModel;

        public HomeViewModelFactory(ISimpleTraderViewModelFactory<MajorIndexListingViewModel> majorIndexListingViewModel)
        {
            _majorIndexListingViewModel = majorIndexListingViewModel;
        }

        public HomeViewModel CreateViewModel()
        {
            return new HomeViewModel(_majorIndexListingViewModel.CreateViewModel());
        }
    }
}
