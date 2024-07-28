using MVVMEssentials.ViewModels;
namespace StateMVVM.ViewModels
{
    public class LayoutViewModel : ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public ViewModelBase ContentViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel, GlobalMessageViewModel globalMessageViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
            GlobalMessageViewModel = globalMessageViewModel;
        }
        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            GlobalMessageViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}
