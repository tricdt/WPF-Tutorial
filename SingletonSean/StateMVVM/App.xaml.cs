using StateMVVM.Services.Navigations;
using StateMVVM.Stores;
using StateMVVM.ViewModels;
using System.Windows;
namespace StateMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly PostStore _postStore;
        public App()
        {
            _navigationStore = new NavigationStore();
            _postStore = new PostStore(new Services.PostService());
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService navigationService = CreatePostHomeNavigationService();
            navigationService.Navigate();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        private INavigationService CreatePostHomeNavigationService()
        {
            return new LayoutNavigationService<PostHomeViewModel>(_navigationStore,
                CreatePostHomeViewModel,
                CreateNavigationBarViewModel);
        }
        private PostHomeViewModel CreatePostHomeViewModel()
        {
            return new PostHomeViewModel(new CreatePostViewModel(_postStore), RecentPostListingViewModel.LoadViewModel(_postStore));
        }
        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            return new NavigationBarViewModel(
                CreatePostHomeNavigationService(),
                CreatePostListingNavigationService());
        }


        private INavigationService CreatePostListingNavigationService()
        {
            return new LayoutNavigationService<PostListingViewModel>(_navigationStore,
                CreatePostListingViewModel,
                CreateNavigationBarViewModel);
        }

        private PostListingViewModel CreatePostListingViewModel()
        {
            return PostListingViewModel.LoadViewModel(_postStore);
        }
    }

}
