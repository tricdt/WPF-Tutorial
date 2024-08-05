using Microsoft.EntityFrameworkCore;
using Reservoom.DbContexts;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Services.ReservationConflictValidators;
using Reservoom.Services.ReservationCreators;
using Reservoom.Services.ReservationProviders;
using Reservoom.Stores;
using Reservoom.ViewModels;
using System.Windows;
namespace Reservoom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Hotel _hotel;
        private readonly NavigationStore _navigationStore;
        private const string CONNECTION_STRING = "Data Source=ODEGAARD\\SQLEXPRESS;Initial Catalog=Reservoom;Integrated Security=True;Trust Server Certificate=True";
        private readonly ReservoomDbContextFactory _reservoomDbContextFactory;
        public App()
        {
            _reservoomDbContextFactory = new ReservoomDbContextFactory(CONNECTION_STRING);
            IReservationProvider reservationProvider = new DatabaseReservationProvider(_reservoomDbContextFactory);
            IReservationCreator reservationCreator = new DatabaseReservationCreator(_reservoomDbContextFactory);
            IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_reservoomDbContextFactory);
            ReservationBook reservationBook = new ReservationBook(reservationProvider, reservationCreator, reservationConflictValidator);
            _hotel = new Hotel("SingletonSean Suites", reservationBook);
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options;
            using (ReservoomDbContext dbContext = new ReservoomDbContext(options))
            {
                dbContext.Database.Migrate();
            }
            _navigationStore.CurrentViewModel = CreateMakeReservationViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(_hotel, new NavigationService(_navigationStore, CreateReservationListingViewModel));
        }
        private ReservationListingViewModel CreateReservationListingViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(_hotel, new NavigationService(_navigationStore, CreateMakeReservationViewModel));
        }
    }

}
