using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media.Effects;
namespace syncfusion.demoscommon.wpf
{
    public partial class ProductDemosWindow : ChromelessWindow
    {
        public ProductDemosWindow(DemoBrowserViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.Loaded += ProductDemosWindow_Loaded;
            this.Title = viewModel.SelectedProduct.Product + " Demos";
            if (DemosNavigationService.MainWindow.Width < this.Width ||
                DemosNavigationService.MainWindow.Height < this.Height)
            {
                this.Width = DemosNavigationService.MainWindow.Width * 0.95;
                this.Height = DemosNavigationService.MainWindow.Height * 0.95;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            var navigationService = DemosNavigationService.DemoNavigationService;
            var window = DemosNavigationService.MainWindow;
            window.Effect = null;
            var viewModel = this.DataContext as DemoBrowserViewModel;
            if (viewModel != null)
            {
                viewModel.BlurVisibility = Visibility.Collapsed;
            }
            if (navigationService != null && navigationService.Content != null)
            {
                var demoControl = navigationService.Content as DemoControl;
                if (demoControl != null)
                {
                    demoControl.Dispose();
                    SfSkinManager.Dispose(demoControl);
                }
            }
            DemosNavigationService.DemoNavigationService = null;
            if (viewModel != null)
            {
                viewModel.SelectedProduct = null;
            }
            if (this.TitleBarTemplate != null)
                this.TitleBarTemplate = null;
            this.DataContext = null;
            DemosNavigationService.MainWindow.Activate();
            SfSkinManager.Dispose(this);
            base.OnClosing(e);
        }
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FakeWindowsPeer(this);
        }
        private void ProductDemosWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = DemosNavigationService.MainWindow;
            mainWindow.Effect = new BlurEffect() { RenderingBias = RenderingBias.Quality, KernelType = KernelType.Gaussian, Radius = 5 };
            var viewModel = this.DataContext as DemoBrowserViewModel;
            viewModel.BlurVisibility = Visibility.Visible;
        }
    }
}
