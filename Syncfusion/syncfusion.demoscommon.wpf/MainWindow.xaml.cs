using Syncfusion.Windows.Shared;
using System.Windows;
using System.Windows.Automation.Peers;
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
            this.ROOTFRAME.NavigationService.Navigated += NavigationService_Navigated;
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            if (DemosNavigationService.RootNavigationService.CanGoForward)
            {
                (this.DataContext as DemoBrowserViewModel).SelectedProduct = null;
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FakeWindowsPeer(this);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DemoControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
    public class FakeWindowsPeer : WindowAutomationPeer
    {
        public FakeWindowsPeer(Window window)
            : base(window)
        { }
        protected override List<AutomationPeer> GetChildrenCore()
        {
            return null;
        }
    }
}
