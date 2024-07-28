namespace CollectionViewFilteringMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public EmployeeListingViewModel EmployeeListingViewModel { get; }

        public MainViewModel()
        {
            EmployeeListingViewModel = new EmployeeListingViewModel();
        }
    }
}
