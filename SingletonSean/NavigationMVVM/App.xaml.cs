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
        //private readonly NavigationStore _navigationStore;
        //private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            //_accountStore = new AccountStore();
            //_navigationStore = new NavigationStore();
            //_modalNavigationStore = new ModalNavigationStore();
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<AccountStore>();
            services.AddSingleton<PeopleStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s));
            services.AddSingleton<CloseModalNavigationService>();
            services.AddTransient<HomeViewModel>(s => new HomeViewModel(CreateLoginNavigationService(s)));
            services.AddTransient<AccountViewModel>(s => new AccountViewModel(s.GetRequiredService<AccountStore>(), CreateHomeNavigationService(s)));
            services.AddTransient<LoginViewModel>(CreateLoginViewModel);
            services.AddTransient<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddTransient<PeopleListingViewModel>(s => new PeopleListingViewModel(
              s.GetRequiredService<PeopleStore>(),
              CreateAddPersonNavigationService(s)));
            services.AddTransient<AddPersonViewModel>(s => new AddPersonViewModel(
                s.GetRequiredService<PeopleStore>(),
                s.GetRequiredService<CloseModalNavigationService>()
                ));
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //INavigationService homeNavigationService = CreateHomeNavigationService(serviceProvider);
            //homeNavigationService.Navigate();
            INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }
        private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<HomeViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>(),
                () => CreateNavigationBarViewModel(serviceProvider));
        }

        private INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
        {
            //return new NavigationService<LoginViewModel>(
            //    _navigationStore,
            //    () => new LoginViewModel(_accountStore, CreateAccountNavigationService()));
            CompositeNavigationService navigationService = new CompositeNavigationService(
                new CloseModalNavigationService(serviceProvider.GetRequiredService<ModalNavigationStore>()),
                CreateAccountNavigationService(serviceProvider));
            return new ModalNavigationService<LoginViewModel>(serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => new LoginViewModel(serviceProvider.GetRequiredService<AccountStore>(), navigationService));
        }

        private INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<AccountViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<AccountViewModel>(),
                () => CreateNavigationBarViewModel(serviceProvider));
        }
        private LoginViewModel CreateLoginViewModel(IServiceProvider serviceProvider)
        {
            CompositeNavigationService navigationService = new CompositeNavigationService(serviceProvider.GetRequiredService<CloseModalNavigationService>(),
                CreateAccountNavigationService(serviceProvider));
            return new LoginViewModel(serviceProvider.GetRequiredService<AccountStore>(), navigationService);
        }

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(serviceProvider.GetRequiredService<AccountStore>(),
                                               CreateHomeNavigationService(serviceProvider),
                                               CreateAccountNavigationService(serviceProvider),
                                               CreateLoginNavigationService(serviceProvider),
                                               CreatePeopleListingNavigationService(serviceProvider)
                                               );
        }

        private INavigationService CreatePeopleListingNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<PeopleListingViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<PeopleListingViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }
        private INavigationService CreateAddPersonNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<AddPersonViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<AddPersonViewModel>());
        }
    }

}
