using Syncfusion.Windows.Shared;
using System.Windows;
using System.Windows.Navigation;
namespace syncfusion.demoscommon.wpf
{
    public static class DemosNavigationService
    {
        public static NavigationService RootNavigationService { get; set; }
        public static NavigationService DemoNavigationService { get; set; }
        public static Window MainWindow { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow(DemoBrowserViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            DemosNavigationService.MainWindow = this;
            DemosNavigationService.RootNavigationService = this.ROOTFRAME.NavigationService;
            DemosNavigationService.RootNavigationService.Navigate(new ProductsListView() { DataContext = viewModel });
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DemoControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
