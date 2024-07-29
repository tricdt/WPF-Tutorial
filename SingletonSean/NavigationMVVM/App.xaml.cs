using Microsoft.Extensions.DependencyInjection;
using NavigationMVVM.Services;
using NavigationMVVM.Stores;
using NavigationMVVM.ViewModels;
using System.Windows;
namespace NavigationMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private readonly AccountStore _accountStore;
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            //_accountStore = new AccountStore();
            _navigationStore = new NavigationStore();
            _modalNavigationStore = new ModalNavigationStore();
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<AccountStore>();
            services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s));
            _serviceProvider = services.BuildServiceProvider();
        }
        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(serviceProvider.GetRequiredService<AccountStore>(),
                                               CreateHomeNavigationService(serviceProvider),
                                               CreateAccountNavigationService(serviceProvider),
                                               CreateLoginNavigationService(serviceProvider)
                                               );
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            //INavigationService homeNavigationService = CreateHomeNavigationService(serviceProvider);
            //homeNavigationService.Navigate();
            INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore, _modalNavigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
        private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<HomeViewModel>(
                _navigationStore,
                () => new HomeViewModel(CreateLoginNavigationService(serviceProvider)), () => CreateNavigationBarViewModel(serviceProvider));
        }

        private INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
        {
            //return new NavigationService<LoginViewModel>(
            //    _navigationStore,
            //    () => new LoginViewModel(_accountStore, CreateAccountNavigationService()));
            CompositeNavigationService navigationService = new CompositeNavigationService(
                new CloseModalNavigationService(_modalNavigationStore),
                CreateAccountNavigationService(serviceProvider));
            return new ModalNavigationService<LoginViewModel>(_modalNavigationStore, () => new LoginViewModel(serviceProvider.GetRequiredService<AccountStore>(), navigationService));
        }

        private INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<AccountViewModel>(
                _navigationStore,
                () => new AccountViewModel(serviceProvider.GetRequiredService<AccountStore>(), CreateHomeNavigationService(serviceProvider)), () => CreateNavigationBarViewModel(serviceProvider));
        }
    }

}
